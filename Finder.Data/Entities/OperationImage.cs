using EntityFrameworkCore.RepositoryInfrastructure;

namespace Finder.Data.Entities;

public class OperationImage : EntityBase, IEntity
{
    public int Position { get; set; }

    public string ImageUrl { get; set; } = null!;

    public string ImageThumbnailUrl { get; set; } = null!;

    public Guid OperationId { get; set; }
}