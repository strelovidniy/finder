﻿using EntityFrameworkCore.RepositoryInfrastructure;

namespace Finder.Data.Entities;

public class NotificationSettings : EntityBase, IEntity
{
    public Guid UserId { get; set; }

    public bool EnableNotifications { get; set; }

    public bool EnableTagFiltration { get; set; }

    public List<string>? FilterTags { get; set; }

    public bool EnableTitleFiltration { get; set; }

    public List<string>? FilterTitles { get; set; }

    public bool EnableUpdateNotifications { get; set; }

    public User? User { get; set; }
}