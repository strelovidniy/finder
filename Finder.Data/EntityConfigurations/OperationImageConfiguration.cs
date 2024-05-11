using Finder.Data.Entities;
using Finder.Data.Enums.RichEnums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Finder.Data.EntityConfigurations;

internal class OperationImageConfiguration : IEntityTypeConfiguration<OperationImage>
{
    public void Configure(EntityTypeBuilder<OperationImage> builder)
    {
        builder
            .ToTable(TableName.HelpRequestImages, TableSchema.Dbo);

        builder
            .HasKey(helpRequestImage => helpRequestImage.Id);

        builder
            .Property(helpRequestImage => helpRequestImage.Id)
            .HasDefaultValueSql(DefaultSqlValue.NewGuid);

        builder
            .Property(helpRequestImage => helpRequestImage.CreatedAt)
            .HasDefaultValueSql(DefaultSqlValue.NowUtc);

        builder
            .Property(helpRequestImage => helpRequestImage.UpdatedAt)
            .IsRequired(false);

        builder
            .Property(helpRequestImage => helpRequestImage.DeletedAt)
            .IsRequired(false);

        builder
            .Property(helpRequestImage => helpRequestImage.ImageThumbnailUrl)
            .HasMaxLength(200)
            .IsRequired();

        builder
            .Property(helpRequestImage => helpRequestImage.ImageUrl)
            .HasMaxLength(200)
            .IsRequired();

        builder
            .Property(helpRequestImage => helpRequestImage.Position)
            .IsRequired();
    }
}