namespace Finder.Domain.Settings.Abstraction;

internal interface IEmailSettings
{
    public string Server { get; set; }

    public int Port { get; set; }

    public bool UseSSL { get; set; }

    public string Password { get; set; }

    public string FromEmail { get; set; }

    public string FromDisplayName { get; set; }
}