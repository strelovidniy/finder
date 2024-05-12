using Discord;
using Finder.Domain.Constants;
using Finder.Domain.Helpers;
using Finder.Domain.Services.Abstraction;
using Finder.Domain.Settings.Abstraction;

namespace Finder.Domain.Services.Realization;

public class ChatService(IDiscordClientService discordClientService, IChatSettings chatSettings) : IChatService
{
    public async Task<(ulong ChannelId, string InviteLink)> CreateChannelAsync(string channelTitle)
    {
        var guildDetails = await discordClientService.PerformActionWhenReady<(ulong ChannelId, string InviteLink)>(async client => 
        {
            var guild = client.GetGuild(chatSettings.ServerGuildId);

            if (guild == null)
            {
                return (0, null)!;
            }
            
            var channel = await guild.CreateTextChannelAsync(channelTitle);

            var everyoneRole = guild.EveryoneRole;
            await channel.AddPermissionOverwriteAsync(everyoneRole, new OverwritePermissions(createInstantInvite: PermValue.Deny));

            var adminRole = guild.Roles.FirstOrDefault(r => r.Name == Defaults.ChatAdminRole);
            if (adminRole != null)
            {
                await channel.AddPermissionOverwriteAsync(adminRole, new OverwritePermissions(createInstantInvite: PermValue.Allow));
            }
            
            var builder = WelcomeMessageHelper.GetWelcomeMessage(channel);
            await channel.SendMessageAsync(builder.ToString());
            var inviteLink = await channel.CreateInviteAsync(maxAge: 86400, maxUses: 10, isUnique: true);
            
            return (ChannelId: channel.Id, InviteLink: inviteLink.Url);
        });

        return guildDetails;
    }
}