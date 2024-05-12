using Finder.Data.Entities;
using Finder.Data.Enums.RichEnums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Finder.Data.EntityConfigurations;

internal class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder
            .ToTable(TableName.Roles, TableSchema.Dbo);

        builder
            .HasKey(role => role.Id);

        builder
            .Property(role => role.Id)
            .HasDefaultValueSql(DefaultSqlValue.NewGuid);

        builder
            .Property(role => role.CreatedAt)
            .HasDefaultValueSql(DefaultSqlValue.NowUtc);

        builder
            .Property(role => role.UpdatedAt)
            .IsRequired(false);

        builder
            .Property(role => role.DeletedAt)
            .IsRequired(false);

        builder
            .Property(role => role.IsHidden)
            .IsRequired()
            .HasDefaultValue(false);

        builder
            .Property(role => role.CanDeleteUsers)
            .IsRequired()
            .HasDefaultValue(false);

        builder
            .Property(role => role.CanRestoreUsers)
            .IsRequired()
            .HasDefaultValue(false);

        builder
            .Property(role => role.CanEditUsers)
            .IsRequired()
            .HasDefaultValue(false);

        builder
            .Property(role => role.CanCreateRoles)
            .IsRequired()
            .HasDefaultValue(false);

        builder
            .Property(role => role.CanEditRoles)
            .IsRequired()
            .HasDefaultValue(false);

        builder
            .Property(role => role.CanDeleteRoles)
            .IsRequired()
            .HasDefaultValue(false);

        builder
            .Property(role => role.CanSeeAllUsers)
            .IsRequired()
            .HasDefaultValue(false);

        builder
            .Property(role => role.CanSeeUsers)
            .IsRequired()
            .HasDefaultValue(false);

        builder
            .Property(role => role.CanSeeAllRoles)
            .IsRequired()
            .HasDefaultValue(false);

        builder
            .Property(role => role.CanSeeRoles)
            .IsRequired()
            .HasDefaultValue(false);

        builder
            .Property(role => role.CanMaintainSystem)
            .IsRequired()
            .HasDefaultValue(false);

        builder
            .Property(role => role.CanCreateSearchOperation)
            .IsRequired()
            .HasDefaultValue(false);

        builder
            .Property(role => role.CanSeeHelpRequests)
            .IsRequired()
            .HasDefaultValue(false);

        builder
            .Property(role => role.Type)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder
            .Property(role => role.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder
            .HasIndex(role => role.Name)
            .IsUnique();
    }
}