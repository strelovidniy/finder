using Finder.Data.DependencyInjection;
using Finder.Domain.DependencyInjection;
using Finder.Domain.Extensions;
using Finder.Domain.Helpers;
using Finder.Domain.Json.Converters;
using Finder.Domain.Middleware;
using Finder.Domain.Settings.Realization;
using Finder.Server.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Serilog;

namespace Finder.Server.DependencyInjection;

public static class DependencyInjectionExtension
{
    private const string MainPolicyName = "_main_policy_";

    public static IServiceCollection RegisterApplication(
        this IServiceCollection services,
        IConfiguration configuration
    ) => services
        .ConfigureKestrel()
        .RegisterLogging()
        .AddMvc()
        .Services
        .RegisterDataLayer(configuration)
        .RegisterDomainLayer(configuration)
        .RegisterWebApi(configuration);

    private static IServiceCollection ConfigureKestrel(this IServiceCollection services) =>
        services.Configure<KestrelServerOptions>(options =>
        {
            options.Limits.MaxRequestBodySize = 4294967295; // Max for IIS
            options.Limits.MaxRequestBufferSize = null;
        });

    private static IServiceCollection RegisterLogging(this IServiceCollection services) =>
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.SetMinimumLevel(LogLevel.Trace);
            loggingBuilder.AddSerilog(Log.Logger);
        });

    private static IServiceCollection RegisterWebApi(
        this IServiceCollection services,
        IConfiguration configuration
    ) => services
        .RegisterCors(configuration)
        .RegisterControllers(configuration)
        .RegisterAuth(configuration)
        .RegisterSwagger();

    private static IServiceCollection RegisterCors(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var authSettings = new AuthSettings();

        configuration.GetSection("AuthSettings").Bind(authSettings);

        return services.AddCors(options => options.AddPolicy(
            MainPolicyName,
            policyBuilder => policyBuilder
                .WithOrigins(authSettings.AllowedOrigins.ToNullSafeArray())
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()));
    }

    private static IServiceCollection RegisterAuth(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var jwtSettings = new JwtSettings();

        configuration.GetSection(nameof(JwtSettings)).Bind(jwtSettings);

        return services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x => x.TokenValidationParameters = JwtHelper.GetTokenValidationParameters(jwtSettings))
            .Services
            .AddAuthorization();
    }

    private static IServiceCollection RegisterSwagger(
        this IServiceCollection services
    ) => services.AddSwaggerGen(swaggerGenOptions =>
    {
        swaggerGenOptions.SwaggerDoc(
            "v1",
            new OpenApiInfo
            {
                Title = "AI Seller Web API",
                Version = "v1"
            }
        );

        swaggerGenOptions.AddSecurityDefinition(
            "Bearer",
            new OpenApiSecurityScheme
            {
                Description = "Example: \"Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            }
        );

        swaggerGenOptions.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header
                },
                new List<string>()
            }
        });
    });

    private static IServiceCollection RegisterControllers(
        this IServiceCollection services,
        IConfiguration configuration
    ) =>
        services
            .AddControllersWithViews(options =>
            {
                options.Filters.Add<ApiExceptionFilter>();
                options.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();

                options.CacheProfiles.Add(
                    ResponseCacheProfile.StaticDataCacheProfile,
                    new CacheProfile
                    {
                        Duration = int.Parse(
                            configuration.GetSection("ResponseCaching")["StaticDataCacheDurationSeconds"]!
                        )
                    }
                );
            })
            .AddCustomBadRequest()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;

                options.SerializerSettings.Converters.Add(
                    new JsonDateTimeConverter()
                );

                options.SerializerSettings.Converters.Add(
                    new StringEnumConverter(
                        new CamelCaseNamingStrategy(),
                        false
                    )
                );
            })
            .Services;


    private static IMvcBuilder AddCustomBadRequest(this IMvcBuilder builder)
    {
        builder.ConfigureApiBehaviorOptions(options =>
            options.InvalidModelStateResponseFactory = actionContext =>
            {
                var allErrors = actionContext.ModelState.Values.SelectMany(v => v.Errors);

                return new BadRequestObjectResult(new
                {
                    StatusCode = 400,
                    Message = string.Join(" - ", allErrors.Select(e => e.ErrorMessage))
                });
            });

        return builder;
    }

    private static void UseCorsConfiguration(this IApplicationBuilder app) =>
        app
            .UseCors(MainPolicyName);

    private static void UseAuthConfiguration(this IApplicationBuilder app) =>
        app
            .UseAuthentication()
            .UseAuthorization();

    public static IApplicationBuilder UseApplication(this WebApplication app)
    {
        app.UseCorsConfiguration();

        if (app.Environment.IsProduction())
        {
            app.UseHsts();
        }
        else
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseDefaultFiles();
        app.UseStaticFiles();
        app.UseRouting();

        app.UseAuthConfiguration();

        app.MapControllers();

        app.MapFallbackToFile("index.html");

        return app;
    }
}