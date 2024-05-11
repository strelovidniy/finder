using Finder.Data.Enums;

namespace Finder.Domain.Models.Views;

public record AccessView(
    string Name,
    RoleType Type,
    bool CanDeleteUsers,
    bool CanEditUsers,
    bool CanRestoreUsers,
    bool CanCreateRoles,
    bool CanEditRoles,
    bool CanDeleteRoles,
    bool CanSeeAllUsers,
    bool CanSeeUsers,
    bool CanSeeAllRoles,
    bool CanSeeRoles,
    bool CanMaintainSystem,
    bool CanCreateHelpRequest,
    bool CanSeeHelpRequests
);