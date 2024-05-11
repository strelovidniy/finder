using Finder.Data.Entities;

namespace Finder.Domain.Services.Abstraction;

public interface ICurrentUserService
{
    public Task<User> GetCurrentUserAsync(
        CancellationToken cancellationToken = default
    );
}