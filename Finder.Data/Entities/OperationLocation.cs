using EntityFrameworkCore.RepositoryInfrastructure;

namespace Finder.Data.Entities;

public class OperationLocation : EntityBase, IEntity
{
    public Guid Id { get; set; }

    public Guid SearchOperationId { get; set; }

    public SearchOperation SearchOperation { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;
}