using Finder.Domain.Extensions;
using Finder.Domain.Settings.Abstraction;
using Microsoft.IdentityModel.Tokens;

namespace Finder.Domain.Helpers;

public static class JwtHelper
{
    public static TokenValidationParameters GetTokenValidationParameters(
        IJwtSettings jwtSettings
    ) => new()
    {
        ValidateIssuer = true,
        ValidIssuers = new[] { jwtSettings.Issuer },

        ValidateAudience = false,

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(jwtSettings.TokenSecretString.ToByteArray()),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
}