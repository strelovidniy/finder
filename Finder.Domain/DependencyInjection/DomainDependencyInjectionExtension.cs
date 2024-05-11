using Finder.Domain.Mapper.Profiles;
using Finder.Domain.Models;
using Finder.Domain.Models.Change;
using Finder.Domain.Models.Create;
using Finder.Domain.Models.Set;
using Finder.Domain.Models.Update;
using Finder.Domain.Services.Abstraction;
using Finder.Domain.Services.Realization;
using Finder.Domain.Settings.Abstraction;
using Finder.Domain.Settings.Realization;
using Finder.Domain.Validators;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Finder.Domain.DependencyInjection;

public static class DomainDependencyInjectionExtension
{
    public static IServiceCollection RegisterDomainLayer(
        this IServiceCollection services,
        IConfiguration configuration
    ) => services
        .AddServices()
        .AddValidators()
        .AddSettings(configuration)
        .AddMapper();

    private static IServiceCollection AddServices(
        this IServiceCollection services
    ) => services
        .AddHttpContextAccessor()
        .AddScoped<IValidationService, ValidationService>()
        .AddScoped<IUserService, UserService>()
        .AddScoped<IRoleService, RoleService>()
        .AddScoped<IRazorViewToStringRenderer, RazorViewToStringRenderer>()
        .AddScoped<IEmailService, EmailService>()
        .AddScoped<ICurrentUserService, CurrentUserService>()
        .AddScoped<IUserAccessService, UserAccessService>()
        .AddScoped<IUserDetailsService, UserDetailsService>()
        .AddScoped<IStorageService, StorageService>()
        .AddScoped<ISearchOperationService, SearchOperationService>()
        .AddScoped<IImageService, ImageService>()
        .AddScoped<INotificationService, NotificationService>()
        .AddScoped<INotificationSettingsService, NotificationSettingsService>()
        .AddScoped<ISearchOperationService, SearchOperationService>()
        .AddScoped<IPushSubscriptionService, PushSubscriptionService>()
        .AddScoped<ISearchOperationNotificationService, SearchOperationNotificationService>()
        .AddScoped<IQrGenerationService, QrGenerationService>();

    private static IServiceCollection AddValidators(
        this IServiceCollection services
    ) => services
        .AddValidator<LoginModel, LoginModelValidator>()
        .AddValidator<ChangePasswordModel, ChangePasswordModelValidator>()
        .AddValidator<CreateRoleModel, CreateRoleModelValidator>()
        .AddValidator<CreateUserModel, CreateUserModelValidator>()
        .AddValidator<RegisterUserModel, RegisterUserModelValidator>()
        .AddValidator<ResetPasswordModel, ResetPasswordModelValidator>()
        .AddValidator<SetNewPasswordModel, SetNewPasswordModelValidator>()
        .AddValidator<UpdateRoleModel, UpdateRoleModelValidator>()
        .AddValidator<UpdateUserModel, UpdateUserModelValidator>()
        .AddValidator<ChangeUserRoleModel, ChangeUserRolesModelValidator>()
        .AddValidator<UpdateProfileModel, UpdateProfileModelValidator>()
        .AddValidator<CompleteRegistrationModel, CompleteRegistrationModelValidator>()
        .AddValidator<UpdateAddressModel, UpdateAddressModelValidator>()
        .AddValidator<SetUserAvatarModel, SetUserAvatarModelValidator>()
        .AddValidator<UpdateContactInfoModel, UpdateContactInfoModelValidator>()
        .AddValidator<CreatePushSubscriptionModel, CreatePushSubscriptionModelValidator>()
        .AddValidator<CreatePushSubscriptionKeysModel, CreatePushSubscriptionKeysModelValidator>()
        .AddValidator<UpdateNotificationSettingModel, UpdateNotificationSettingModelValidator>()
        .AddValidator<CreateSearchOperationRequestModel, CreateSearchOperationModelValidator>()
        .AddValidator<UpdateSearchOperationRequestModel, UpdateSearchOperationModelValidator>();

    private static IServiceCollection AddValidator<TModel, TValidator>(
        this IServiceCollection services
    )
        where TModel : class, IValidatableModel
        where TValidator : class, IValidator<TModel> => services
        .AddTransient<IValidator<TModel>, TValidator>();

    private static IServiceCollection AddMapper(
        this IServiceCollection services
    ) => services
        .AddAutoMapper(config => config.AddProfiles(
        [
            new AddressMapperProfile(),
            new ContactInfoMapperProfile(),
            new NotificationSettingsMapperProfile(),
            new RoleMapperProfile(),
            new UserDetailsMapperProfile(),
            new OperationImageMapperProfile(),
            new OperationMapperProfile(),
            new UserMapperProfile()
        ]));

    private static IServiceCollection AddSettings(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var emailSettings = new EmailSettings();
        var urlSettings = new UrlSettings();
        var jwtSettings = new JwtSettings();
        var imageSettings = new ImageSettings();
        var webPushSettings = new WebPushSettings();

        configuration
            .GetSection(nameof(EmailSettings))
            .Bind(emailSettings);

        configuration
            .GetSection(nameof(UrlSettings))
            .Bind(urlSettings);

        configuration
            .GetSection(nameof(JwtSettings))
            .Bind(jwtSettings);

        configuration
            .GetSection(nameof(ImageSettings))
            .Bind(imageSettings);

        configuration
            .GetSection(nameof(WebPushSettings))
            .Bind(webPushSettings);

        services
            .AddSingleton<IEmailSettings, EmailSettings>(_ => emailSettings)
            .AddSingleton<IUrlSettings, UrlSettings>(_ => urlSettings)
            .AddSingleton<IJwtSettings, JwtSettings>(_ => jwtSettings)
            .AddSingleton<IImageSettings, ImageSettings>(_ => imageSettings)
            .AddSingleton<IWebPushSettings, WebPushSettings>(_ => webPushSettings);

        return services;
    }
}