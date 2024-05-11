using Finder.Domain.Models.Update;

namespace Finder.Domain.Services.Abstraction;

public interface INotificationSettingsService
{
    public Task UpdateNotificationSettingAsync(
        UpdateNotificationSettingModel updateNotificationSettingModel,
        CancellationToken cancellationToken = default
    );
}