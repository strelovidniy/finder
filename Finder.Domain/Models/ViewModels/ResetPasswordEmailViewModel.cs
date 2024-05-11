namespace Finder.Domain.Models.ViewModels;

public record ResetPasswordEmailViewModel(
    string Url
) : IEmailViewModel;