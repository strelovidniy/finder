using Finder.Domain.Settings.Abstraction;

namespace Finder.Domain.Settings.Realization;

internal class WebPushSettings : IWebPushSettings
{
    public string PublicKey { get; set; } = null!;

    public string PrivateKey { get; set; } = null!;
}