using BotDiscord.Comandos.Join;
using Discord;
using Discord.Audio;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace BotDiscord.Comandos.Disconnect
{
    public class DisconnectChannel : ModuleBase<SocketCommandContext>
    {
        [Command("Disconnect", RunMode = RunMode.Async)]
        public async Task LeaveChannel(IVoiceChannel channel = null)
        {
            channel = channel ?? (Context.User as IGuildUser)?.VoiceChannel;
            if (channel == null)
            {
                await Context.Channel.SendMessageAsync("O usuário precisa estar conectado a um chat de voz.");
                return;
            }
            await Context.Channel.SendMessageAsync("Bye bye.");
            await channel.DisconnectAsync();
        }
    }
}
