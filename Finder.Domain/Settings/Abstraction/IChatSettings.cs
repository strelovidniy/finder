namespace Finder.Domain.Settings.Abstraction;

public interface IChatSettings
{
    public string DiscordBotToken { get; set; }

    public ulong ServerGuildId { get; set; }
}