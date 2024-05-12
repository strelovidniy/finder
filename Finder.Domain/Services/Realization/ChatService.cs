using Finder.Domain.Services.Abstraction;
using Finder.Domain.Settings.Abstraction;

namespace Finder.Domain.Services.Realization;

public class ChatService(IDiscordClientService discordClientService, IChatSettings chatSettings) : IChatService
{
    public async Task<(ulong ChannelId, string InviteLink)> CreateChannelAsync(string channelTitle)
    {
        var guildDetails = await discordClientService.PerformActionWhenReady<(ulong ChannelId, string InviteLink)>(
            async client =>
            {
                var guild = client.GetGuild(chatSettings.ServerGuildId);

                if (guild == null)
                {
                    return (0, null)!;
                }

                var channel = await guild.CreateTextChannelAsync(channelTitle);
                var inviteLink = await channel.CreateInviteAsync(86400, 10);

                return (ChannelId: channel.Id, InviteLink: inviteLink.Url);
            });

        return guildDetails;
    }
}