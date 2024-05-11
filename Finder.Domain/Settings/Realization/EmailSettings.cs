using Finder.Domain.Settings.Abstraction;

namespace Finder.Domain.Settings.Realization;

internal class EmailSettings : IEmailSettings
{
    public string Server { get; set; } = null!;

    public int Port { get; set; }

    public bool UseSSL { get; set; }

    public string Password { get; set; } = null!;

    public string FromEmail { get; set; } = null!;

    public string FromDisplayName { get; set; } = null!;
}