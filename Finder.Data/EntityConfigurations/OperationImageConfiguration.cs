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
            .ToTable(TableName.OperationImage, TableSchema.Dbo);

        builder
            .HasKey(operationImage => operationImage.Id);

        builder
            .Property(operationImage => operationImage.Id)
            .HasDefaultValueSql(DefaultSqlValue.NewGuid);

        builder
            .Property(operationImage => operationImage.CreatedAt)
            .HasDefaultValueSql(DefaultSqlValue.NowUtc);

        builder
            .Property(operationImage => operationImage.UpdatedAt)
            .IsRequired(false);

        builder
            .Property(operationImage => operationImage.DeletedAt)
            .IsRequired(false);

        builder
            .Property(operationImage => operationImage.ImageThumbnailUrl)
            .HasMaxLength(200)
            .IsRequired();

        builder
            .Property(operationImage => operationImage.ImageUrl)
            .HasMaxLength(200)
            .IsRequired();

        builder
            .Property(operationImage => operationImage.Position)
            .IsRequired();
    }
}