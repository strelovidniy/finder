namespace Finder.Domain.Models.Set;

public record SetNewPasswordModel(
    Guid VerificationCode,
    string NewPassword,
    string ConfirmNewPassword
) : IValidatableModel;