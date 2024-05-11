using Finder.Data.Entities;
using Finder.Data.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Finder.Data.Extensions.Seed;

internal static class Roles
{
    public static void Seed(EntityTypeBuilder<Role> builder)
    {
        builder.HasData(
            new Role
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Name = "Admin",
                CreatedAt = new DateTime(2022, 11, 11, 1, 6, 0, DateTimeKind.Utc),
                Type = RoleType.Admin,
                IsHidden = true,
                CanCreateRoles = true,
                CanDeleteRoles = true,
                CanDeleteUsers = true,
                CanRestoreUsers = true,
                CanEditRoles = true,
                CanEditUsers = true,
                CanSeeAllUsers = true,
                CanSeeAllRoles = true,
                CanSeeRoles = true,
                CanSeeUsers = true,
                CanMaintainSystem = true,
                CanCreateHelpRequest = true,
                CanSeeHelpRequests = true
            },
            new Role
            {
                Id = Guid.Parse("a0a80c03-abbc-eb11-cabb-0022480a1c0a"),
                Name = "User",
                CreatedAt = new DateTime(2022, 11, 11, 1, 6, 0, DateTimeKind.Utc),
                Type = RoleType.User,
                IsHidden = true,
                CanCreateHelpRequest = true
            }
        );
    }
}