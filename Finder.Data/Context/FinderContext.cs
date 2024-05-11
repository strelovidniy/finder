using Finder.Data.EntityConfigurations;
using Finder.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Finder.Data.Context;

public class FinderContext : DbContext
{
    public FinderContext(DbContextOptions<FinderContext> options) : base(options)
    {
    }

    public FinderContext()
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AddressConfiguration());
        modelBuilder.ApplyConfiguration(new ContactInfoConfiguration());
        modelBuilder.ApplyConfiguration(new PushSubscriptionConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new SearchOperationConfiguration());
        modelBuilder.ApplyConfiguration(new OperationImageConfiguration());
        modelBuilder.ApplyConfiguration(new UserDetailsConfiguration());

        modelBuilder.Seed();

        base.OnModelCreating(modelBuilder);
    }
}