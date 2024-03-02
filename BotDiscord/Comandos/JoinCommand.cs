using Discord;
using Discord.Audio;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotDiscord.Comandos.Join
{
    public class JoinCommand : ModuleBase<SocketCommandContext>
    {
        [Command("join", RunMode = RunMode.Async)]
        public async Task JoinChannel(IVoiceChannel channel = null)
        {
            channel = channel ?? (Context.User as IGuildUser)?.VoiceChannel;
            if (channel == null)
            {
                await Context.Channel.SendMessageAsync("Entra em um canal primeiro >.<");
                return;
            }
            await Context.Channel.SendMessageAsync("Oieee");
            await channel.ConnectAsync();
        }
    }
}
