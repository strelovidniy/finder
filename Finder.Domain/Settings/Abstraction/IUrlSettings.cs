namespace Finder.Domain.Settings.Abstraction;

internal interface IUrlSettings
{
    public string AppUrl { get; set; }

    public string ResetPasswordUrl { get; set; }

    public string ConfirmEmailUrl { get; set; }

    public string CompleteRegistrationUrl { get; set; }

    public string HelpRequestUrl { get; set; }

    public string WebApiUrl { get; set; }

    public string TwitterUrl { get; set; }

    public string FacebookUrl { get; set; }

    public string InstagramUrl { get; set; }
}