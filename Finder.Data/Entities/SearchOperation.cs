using EntityFrameworkCore.RepositoryInfrastructure;
using Finder.Data.Enums;

namespace Finder.Data.Entities;

public class SearchOperation : EntityBase, IEntity
{
    public Guid UserId { get; set; }

    public User? User { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public SearchOperationType OperationType { get; set; }

    public IEnumerable<string>? Tags { get; set; }
    
    public bool ShowContactInfo { get; set; }
    
    public IEnumerable<OperationImage>? Images { get; set; }
}