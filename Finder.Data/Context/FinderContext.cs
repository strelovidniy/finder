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
        modelBuilder.ApplyConfiguration(new HelpRequestConfiguration());
        modelBuilder.ApplyConfiguration(new HelpRequestImageConfiguration());
        modelBuilder.ApplyConfiguration(new NotificationSettingsConfiguration());
        modelBuilder.ApplyConfiguration(new PushSubscriptionConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new UserDetailsConfiguration());

        modelBuilder.Seed();

        base.OnModelCreating(modelBuilder);
    }
}