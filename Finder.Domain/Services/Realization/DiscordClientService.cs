using Discord;
using Discord.WebSocket;
using Finder.Domain.Services.Abstraction;
using Finder.Domain.Settings.Abstraction;

namespace Finder.Domain.Services.Realization;

public class DiscordClientService : IDiscordClientService
{
    private readonly TaskCompletionSource<bool> _readyCompletionSource = new();

    public DiscordSocketClient Client { get; }

    public DiscordClientService(IChatSettings chatSettings)
    {
        Client = new DiscordSocketClient();
        Client.Ready += OnReady;
        Client.LoginAsync(TokenType.Bot, chatSettings.DiscordBotToken).Wait();
        Client.StartAsync().Wait();
    }

    public async Task<T> PerformActionWhenReady<T>(Func<DiscordSocketClient, Task<T>> action)
    {
        await _readyCompletionSource.Task; // Wait until the client is ready

        return await action(Client);
    }

    private Task OnReady()
    {
        _readyCompletionSource.SetResult(true);

        return Task.CompletedTask;
    }
}