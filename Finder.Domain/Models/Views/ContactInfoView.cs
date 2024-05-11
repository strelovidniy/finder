namespace Finder.Domain.Models.Views;

public record ContactInfoView(
    string? PhoneNumber,
    string? Telegram,
    string? Skype,
    string? LinkedIn,
    string? Instagram,
    string? Other
);