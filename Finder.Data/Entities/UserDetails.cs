using EntityFrameworkCore.RepositoryInfrastructure;

namespace Finder.Data.Entities;

public class UserDetails : EntityBase, IEntity
{
    public Guid UserId { get; set; }

    public string? ImageUrl { get; set; }

    public string? ImageThumbnailUrl { get; set; }

    public Address? Address { get; set; }

    public Guid? AddressId { get; set; }

    public User? User { get; set; }

    public Guid? ContactInfoId { get; set; }

    public ContactInfo? ContactInfo { get; set; }
}