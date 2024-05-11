using Finder.Domain.Settings.Abstraction;

namespace Finder.Domain.Settings.Realization;

public class AuthSettings : IAuthSettings
{
    public IEnumerable<string> AllowedOrigins { get; set; } = null!;
}