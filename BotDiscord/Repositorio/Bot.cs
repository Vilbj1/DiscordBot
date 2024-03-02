using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Discord;
using Discord.Commands;
using Discord.WebSocket;
using BotDiscord.Interfaces;

namespace BotDiscord.Repositorio
{
    public class Bot : IBot
    {
        private ServiceProvider? _serviceProvider;

        private readonly ILogger<Bot> _logger;
        private readonly IConfiguration _configuration;
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        
        public Bot(
            ILogger<Bot> logger, 
            IConfiguration configuration)
        {
            _logger = logger;
             _configuration = configuration;

            DiscordSocketConfig config = new()
            {
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
            };
            
            _client = new DiscordSocketClient(config);
            _commands = new CommandService();
        }
        public async Task StartAsync(ServiceProvider services)
        {
            string discordToken = _configuration["DiscordToken"] ?? throw new Exception("Não encontrou o Discord Token");

            _logger.LogInformation($"Iniciando com o token {discordToken}");

            _serviceProvider = services;
            
            await _commands.AddModulesAsync(Assembly.GetExecutingAssembly(), _serviceProvider);

            await _client.LoginAsync(TokenType.Bot, discordToken);
            await _client.StartAsync();
            
            _client.MessageReceived += HandleCommandAsync;
        }

        public async Task StopAsync()
        {
            _logger.LogInformation("desligando");
            if (_client != null)
            {
                await _client.LogoutAsync();
                await _client.StopAsync();
            }
        }
        private async Task HandleCommandAsync(SocketMessage arg)
        {
            // ignorar fala de outros bots
            if (arg is not SocketUserMessage message || message.Author.IsBot)
            {
                return;
            }

            _logger.LogInformation($"{DateTime.Now.ToShortTimeString()} - {message.Author}: {message.Content}");

            // confirma se a mensagem começa com !
            int position = 0;
            bool messageIsCommand = message.HasCharPrefix('!', ref position);

            if (messageIsCommand)
            {
                await _commands.ExecuteAsync(new SocketCommandContext(_client, message), position, _serviceProvider);
            }
        }
    }
}
