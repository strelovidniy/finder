using Finder.Data.Enums;

namespace Finder.Domain.Models.Create;

public record CreateRoleModel(
    string Name,
    RoleType Type,
    bool CanDeleteUsers = false,
    bool CanRestoreUsers = false,
    bool CanEditUsers = false,
    bool CanCreateRoles = false,
    bool CanEditRoles = false,
    bool CanDeleteRoles = false,
    bool CanSeeAllUsers = false,
    bool CanSeeUsers = false,
    bool CanSeeAllRoles = false,
    bool CanSeeRoles = false,
    bool CanMaintainSystem = false,
    bool CanCreateHelpRequest = false,
    bool CanSeeHelpRequests = false
) : IValidatableModel;