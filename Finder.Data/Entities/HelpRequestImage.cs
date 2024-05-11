using EntityFrameworkCore.RepositoryInfrastructure;

namespace Finder.Data.Entities;

public class HelpRequestImage : EntityBase, IEntity
{
    public int Position { get; set; }

    public string ImageUrl { get; set; } = null!;

    public string ImageThumbnailUrl { get; set; } = null!;

    public Guid HelpRequestId { get; set; }
}