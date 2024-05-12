namespace Finder.Domain.Services.Abstraction;

public interface IChatService
{
    Task<(ulong ChannelId, string InviteLink)> CreateChannelAsync(string channelTitle);
}