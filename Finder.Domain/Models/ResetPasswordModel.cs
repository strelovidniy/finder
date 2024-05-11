namespace Finder.Domain.Models;

public record ResetPasswordModel(
    string Email
) : IValidatableModel;