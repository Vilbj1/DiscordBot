using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace BotDiscord.Interfaces
{
    public interface IBot
    {
        Task StartAsync(ServiceProvider services);
        Task StopAsync();
    }
}
