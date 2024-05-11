using EntityFrameworkCore.RepositoryInfrastructure;
using Finder.Data.Entities;
using Finder.Data.Enums;
using Finder.Domain.Exceptions;
using Finder.Domain.Extensions;
using Finder.Domain.Services.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Finder.Domain.Services.Realization;

internal class CurrentUserService(
    IRepository<User> userRepository,
    IHttpContextAccessor httpContextAccessor
) : ICurrentUserService
{
    public async Task<User> GetCurrentUserAsync(
        CancellationToken cancellationToken = default
    ) => await userRepository
            .NoTrackingQuery()
            .Include(user => user.Role)
            .FirstOrDefaultAsync(
                user => user.Id == httpContextAccessor.GetCurrentUserId(),
                cancellationToken
            )
        ?? throw new ApiException(StatusCode.Unauthorized);
}