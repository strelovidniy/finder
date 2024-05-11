namespace Finder.Domain.Models;

public record RegisterUserModel(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string ConfirmPassword
) : IValidatableModel;