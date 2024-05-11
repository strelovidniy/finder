using System.Security.Claims;
using Finder.Domain.Exceptions;
using Finder.Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Finder.Server.Controllers.Base;

[ApiController]
[Authorize]
public class BaseController(
    IServiceProvider services
) : ControllerBase
{
    protected Guid CurrentUserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new ApiException(Data.Enums.StatusCode.Unauthorized));

    protected Task ValidateAsync<T>(T validatableModel, CancellationToken cancellationToken = default)
        where T : class, IValidatableModel =>
        services.GetRequiredService<IValidator<T>>().ValidateAndThrowAsync(validatableModel, cancellationToken);
}