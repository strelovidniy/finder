using EntityFrameworkCore.RepositoryInfrastructure;

namespace Finder.Data.Entities;

public class PushSubscription : EntityBase, IEntity
{
    public string Endpoint { get; set; } = null!;

    public DateTime? ExpirationTime { get; set; }

    public string P256dh { get; set; } = null!;

    public string Auth { get; set; } = null!;

    public Guid UserId { get; set; }

    public User? User { get; set; }
}