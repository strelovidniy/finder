namespace Finder.Domain.Models.Create;

public record CreateUserModel(
    Guid RegistrationToken
) : IValidatableModel;