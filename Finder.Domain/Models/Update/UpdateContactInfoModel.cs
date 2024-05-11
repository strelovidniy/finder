namespace Finder.Domain.Models.Update;

public record UpdateContactInfoModel(
    string? PhoneNumber = null,
    string? Telegram = null,
    string? Skype = null,
    string? LinkedIn = null,
    string? Instagram = null,
    string? Other = null
) : IValidatableModel;