using Discord.WebSocket;

namespace Finder.Domain.Services.Abstraction;

public interface IDiscordClientService
{
    Task<T> PerformActionWhenReady<T>(Func<DiscordSocketClient, Task<T>> action);
}