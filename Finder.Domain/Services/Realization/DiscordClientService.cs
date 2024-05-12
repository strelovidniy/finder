using Discord;
using Discord.WebSocket;
using Finder.Domain.Services.Abstraction;
using Finder.Domain.Settings.Abstraction;

namespace Finder.Domain.Services.Realization;

public class DiscordClientService : IDiscordClientService
{
    private readonly DiscordSocketClient _client;
    private TaskCompletionSource<bool> _readyCompletionSource = new TaskCompletionSource<bool>();

    public DiscordClientService(IChatSettings chatSettings)
    {
        _client = new DiscordSocketClient();
        _client.Ready += OnReady;
        _client.LoginAsync(TokenType.Bot, chatSettings.DiscordBotToken).Wait();
        _client.StartAsync().Wait();
    }

    private Task OnReady()
    {
        _readyCompletionSource.SetResult(true);
        return Task.CompletedTask;
    }

    public async Task<T> PerformActionWhenReady<T>(Func<DiscordSocketClient, Task<T>> action)
    {
        await _readyCompletionSource.Task; // Wait until the client is ready
        return await action(_client);
    }

    public DiscordSocketClient Client => _client;
}

