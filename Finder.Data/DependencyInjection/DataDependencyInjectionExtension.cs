using Finder.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Finder.Data.DependencyInjection;

public static class DataDependencyInjectionExtension
{
    public static IServiceCollection RegisterDataLayer(
        this IServiceCollection services,
        IConfiguration configuration
    ) => services
        .AddDbContext(configuration)
        .AddRepositories();

    private static IServiceCollection AddDbContext(
        this IServiceCollection services,
        IConfiguration configuration
    ) => services
        .AddDbContext<VolunterioContext>(options =>
        {
            options
                .UseNpgsql(configuration.GetConnectionString("Volunterio"));

            #if DEBUG
            options
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
            #endif
        });

    private static IServiceCollection AddRepositories(
        this IServiceCollection services
    ) => services
        .CreateRepositoryBuilderWithContext<VolunterioContext>()
        .AddRepository<Address>()
        .AddRepository<ContactInfo>()
        .AddRepository<HelpRequest>()
        .AddRepository<HelpRequestImage>()
        .AddRepository<NotificationSettings>()
        .AddRepository<PushSubscription>()
        .AddRepository<Role>()
        .AddRepository<User>()
        .AddRepository<UserDetails>()
        .Build();
}