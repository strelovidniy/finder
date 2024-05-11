using EntityFrameworkCore.RepositoryInfrastructure;

namespace Finder.Data.Entities;

public class UserSearchOperation : EntityBase, IEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    public Guid SearchOperationId { get; set; }
    public SearchOperation SearchOperation { get; set; }
}