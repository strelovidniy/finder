using EntityFrameworkCore.RepositoryInfrastructure;

namespace Finder.Data.Entities;

public class HelpRequest : EntityBase, IEntity
{
    public Guid IssuerId { get; set; }

    public User? Issuer { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public IEnumerable<string>? Tags { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public bool ShowContactInfo { get; set; }

    public DateTime? Deadline { get; set; }

    public IEnumerable<HelpRequestImage>? Images { get; set; }
}