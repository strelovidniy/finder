namespace Finder.Domain.Settings.Abstraction;

internal interface IWebPushSettings
{
    public string PublicKey { get; set; }

    public string PrivateKey { get; set; }
}