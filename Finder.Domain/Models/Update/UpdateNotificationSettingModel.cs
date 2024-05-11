namespace Finder.Domain.Models.Update;

public record UpdateNotificationSettingModel(
    bool EnableNotifications,
    bool EnableTagFiltration,
    List<string>? FilterTags,
    bool EnableTitleFiltration,
    List<string>? FilterTitles,
    bool EnableUpdateNotifications
) : IValidatableModel;