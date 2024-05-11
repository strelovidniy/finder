using Finder.Domain.Settings.Abstraction;

namespace Finder.Domain.Settings.Realization;

internal class UrlSettings : IUrlSettings
{
    public string AppUrl { get; set; } = null!;

    public string ResetPasswordUrl { get; set; } = null!;

    public string ConfirmEmailUrl { get; set; } = null!;

    public string CompleteRegistrationUrl { get; set; } = null!;

    public string WebApiUrl { get; set; } = null!;

    public string TwitterUrl { get; set; } = null!;

    public string FacebookUrl { get; set; } = null!;

    public string InstagramUrl { get; set; } = null!;
}