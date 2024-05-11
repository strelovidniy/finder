using EntityFrameworkCore.RepositoryInfrastructure;
using Finder.Data.Entities;
using Finder.Domain.Models.Update;
using Finder.Domain.Services.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace Finder.Domain.Services.Realization;

public class NotificationSettingsService(
    IRepository<NotificationSettings> notificationSettingRepository,
    ICurrentUserService currentUserService
) : INotificationSettingsService
{
    public async Task UpdateNotificationSettingAsync(
        UpdateNotificationSettingModel updateNotificationSettingModel,
        CancellationToken cancellationToken = default
    )
    {
        var currentUser = await currentUserService.GetCurrentUserAsync(cancellationToken);

        var notificationSetting = await notificationSettingRepository
            .Query()
            .FirstOrDefaultAsync(
                details => details.UserId == currentUser.Id,
                cancellationToken
            );

        if (notificationSetting is null)
        {
            notificationSetting = await notificationSettingRepository.AddAsync(
                new NotificationSettings
                {
                    UserId = currentUser.Id,
                    EnableNotifications = updateNotificationSettingModel.EnableNotifications,
                    EnableTagFiltration = updateNotificationSettingModel.EnableTagFiltration,
                    FilterTags = updateNotificationSettingModel.FilterTags,
                    EnableTitleFiltration = updateNotificationSettingModel.EnableTitleFiltration,
                    FilterTitles = updateNotificationSettingModel.FilterTitles,
                    EnableUpdateNotifications = updateNotificationSettingModel.EnableUpdateNotifications
                },
                cancellationToken
            );
        }
        else
        {
            if (notificationSetting.EnableNotifications != updateNotificationSettingModel.EnableNotifications)
            {
                notificationSetting.EnableNotifications = updateNotificationSettingModel.EnableNotifications;
                notificationSetting.UpdatedAt = DateTime.UtcNow;
            }

            if (notificationSetting.EnableTagFiltration != updateNotificationSettingModel.EnableTagFiltration)
            {
                notificationSetting.EnableTagFiltration = updateNotificationSettingModel.EnableTagFiltration;
                notificationSetting.UpdatedAt = DateTime.UtcNow;
            }

            if (notificationSetting.FilterTags?.SequenceEqual(updateNotificationSettingModel.FilterTags ?? [])
                is not true)
            {
                notificationSetting.FilterTags = updateNotificationSettingModel.FilterTags;
                notificationSetting.UpdatedAt = DateTime.UtcNow;
            }

            if (notificationSetting.EnableTitleFiltration != updateNotificationSettingModel.EnableTitleFiltration)
            {
                notificationSetting.EnableTitleFiltration = updateNotificationSettingModel.EnableTitleFiltration;
                notificationSetting.UpdatedAt = DateTime.UtcNow;
            }

            if (notificationSetting.FilterTitles?.SequenceEqual(updateNotificationSettingModel.FilterTitles ?? [])
                is not true)
            {
                notificationSetting.FilterTitles = updateNotificationSettingModel.FilterTitles;
                notificationSetting.UpdatedAt = DateTime.UtcNow;
            }

            if (notificationSetting.EnableUpdateNotifications
                != updateNotificationSettingModel.EnableUpdateNotifications)
            {
                notificationSetting.EnableUpdateNotifications
                    = updateNotificationSettingModel.EnableUpdateNotifications;

                notificationSetting.UpdatedAt = DateTime.UtcNow;
            }
        }

        await notificationSettingRepository.SaveChangesAsync(cancellationToken);
    }
}