using Finder.Data.Entities;
using Finder.Data.Extensions.Seed;
using Microsoft.EntityFrameworkCore;

namespace Finder.Data.Extensions;

internal static class SeedDataExtension
{
    public static void Seed(this ModelBuilder builder)
    {
        Roles.Seed(builder.Entity<Role>());
        Users.Seed(builder.Entity<User>());
    }
}