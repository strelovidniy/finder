namespace Finder.Domain.Models.Update;

public record UpdateNotificationSettingModel(
    bool EnableNotifications,
    bool EnableTagFiltration,
    IEnumerable<string>? FilterTags,
    bool EnableTitleFiltration,
    IEnumerable<string>? FilterTitles,
    bool EnableUpdateNotifications
) : IValidatableModel;