using Finder.Domain.Settings.Abstraction;

namespace Finder.Domain.Settings.Realization;

public class ChatSettings : IChatSettings
{
    public string DiscordBotToken { get; set; }
    public ulong ServerGuildId { get; set; }
}