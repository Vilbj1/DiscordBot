using System.Reflection;
using BotDiscord.Interfaces;
using BotDiscord.Repositorio;
using Discord.Audio;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BotDiscord
{
    internal class Program
    {
        private static void Main(string[] args) =>
            MainAsync(args).GetAwaiter().GetResult();
        private static async Task MainAsync(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets(Assembly.GetExecutingAssembly())
                .Build();

            var serviceProvider = new ServiceCollection()
                .AddLogging( options =>
                {
                    options.ClearProviders();
                    options.AddConsole();
                }) 
                .AddSingleton<IConfiguration>(configuration)              
                .AddScoped<IBot, Bot>()
                .BuildServiceProvider();

            try
            {
                IBot bot = serviceProvider.GetRequiredService<IBot>();

                await bot.StartAsync(serviceProvider);

                Console.WriteLine("Conectado no discord");

                do
                {
                    var keyInfo = Console.ReadKey();

                    if (keyInfo.Key == ConsoleKey.Q)
                    {
                        Console.WriteLine("\nDesconectando");

                        await bot.StopAsync();
                        return;
                    }
                } while (true);
            }

            catch (Exception exception)
            {
                Console.WriteLine("Não consegui conectar");
                Environment.Exit(-1);
            }
        }
    }
}