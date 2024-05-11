namespace Finder.Domain.Models.Create;

public record CreatePushSubscriptionKeysModel(
    string P256dh,
    string Auth
) : IValidatableModel;