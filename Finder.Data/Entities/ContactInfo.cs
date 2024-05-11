using EntityFrameworkCore.RepositoryInfrastructure;

namespace Finder.Data.Entities;

public class ContactInfo : EntityBase, IEntity
{
    public string? PhoneNumber { get; set; }

    public string? Telegram { get; set; }

    public string? Skype { get; set; }

    public string? LinkedIn { get; set; }

    public string? Instagram { get; set; }

    public string? Other { get; set; }
}