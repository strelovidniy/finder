using Finder.Data.Entities;
using Finder.Domain.Models;
using Finder.Domain.Models.Create;
using Finder.Domain.Models.Update;
using Finder.Domain.Models.Views;

namespace Finder.Domain.Services.Abstraction;

public interface IRoleService
{
    public Task<PagedCollectionView<Role>> GetRolesAsync(
        QueryParametersModel queryParametersModel,
        CancellationToken cancellationToken = default
    );

    public Task<Role?> GetRoleAsync(
        Guid id,
        CancellationToken cancellationToken = default
    );

    public Task<Role?> GetUserRoleAsync(
        CancellationToken cancellationToken = default
    );

    public Task<Role> UpdateRoleAsync(
        UpdateRoleModel updateRoleModel,
        CancellationToken cancellationToken = default
    );

    public Task<Role> CreateRoleAsync(
        CreateRoleModel createRoleModel,
        CancellationToken cancellationToken = default
    );

    public Task DeleteRoleAsync(
        Guid id,
        CancellationToken cancellationToken = default
    );
}