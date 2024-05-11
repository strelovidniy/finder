using EntityFrameworkCore.RepositoryInfrastructure;
using Finder.Data.Enums;

namespace Finder.Data.Entities;

public class SearchOperation : EntityBase, IEntity
{
    public Guid CreatorUserId { get; set; }

    public User? Creator { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public SearchOperationType OperationType { get; set; }

    public IEnumerable<string>? Tags { get; set; }
    
    public bool ShowContactInfo { get; set; }
    
    public SearchOperationStatus OperationStatus { get; set; }
    
    public IEnumerable<OperationImage>? Images { get; set; }
    
    public ICollection<UserSearchOperation> UserApplications { get; set; } = new List<UserSearchOperation>();
}