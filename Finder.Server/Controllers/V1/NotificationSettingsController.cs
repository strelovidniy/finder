using Finder.Domain.Attributes;
using Finder.Domain.Models.Update;
using Finder.Domain.Services.Abstraction;
using Finder.Server.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace Finder.Server.Controllers.V1;

[RouteV1("notification-settings")]
public class NotificationSettingsController(
    IServiceProvider services,
    IUserAccessService userAccessService,
    INotificationSettingsService notificationSettingsService
) : BaseController(services)
{
    [HttpPut("update")]
    public async Task<IActionResult> UpdateNotificationSettingAsync(
        [FromBody] UpdateNotificationSettingModel model,
        CancellationToken cancellationToken = default
    )
    {
        await userAccessService.CheckIfUserCanSeeHelpRequests(cancellationToken);

        await ValidateAsync(model, cancellationToken);

        await notificationSettingsService.UpdateNotificationSettingAsync(model, cancellationToken);

        return Ok();
    }
}