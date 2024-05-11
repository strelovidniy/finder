namespace Finder.Domain.Models.Create;

public record CreatePushSubscriptionModel(
    string Endpoint,
    DateTime? ExpirationTime,
    CreatePushSubscriptionKeysModel Keys
) : IValidatableModel;