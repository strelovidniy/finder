using AutoMapper;
using Finder.Domain.Models.Views;

namespace Finder.Domain.Mapper.Converters.Role;

internal class RoleToAccessViewConverter : ITypeConverter<Data.Entities.Role, AccessView>
{
    public AccessView Convert(
        Data.Entities.Role role,
        AccessView accessView,
        ResolutionContext context
    ) => new(
        role.Name,
        role.Type,
        role.CanDeleteUsers,
        role.CanEditUsers,
        role.CanRestoreUsers,
        role.CanCreateRoles,
        role.CanEditRoles,
        role.CanDeleteRoles,
        role.CanSeeAllUsers,
        role.CanSeeUsers,
        role.CanSeeAllRoles,
        role.CanSeeRoles,
        role.CanMaintainSystem,
        role.CanCreateSearchOperation,
        role.CanSeeHelpRequests
    );
}