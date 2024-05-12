using Finder.Data.Entities;
using Finder.Data.Enums.RichEnums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Finder.Data.EntityConfigurations;

internal class OperationLocationConfiguration : IEntityTypeConfiguration<OperationLocation>
{
    public void Configure(EntityTypeBuilder<OperationLocation> builder)
    {
        builder
            .ToTable(TableName.OperationLocation, TableSchema.Dbo);

        builder
            .HasKey(operationLocation => operationLocation.Id);

        builder
            .Property(operationLocation => operationLocation.Id)
            .HasDefaultValueSql(DefaultSqlValue.NewGuid);

        builder
            .Property(operationLocation => operationLocation.CreatedAt)
            .HasDefaultValueSql(DefaultSqlValue.NowUtc);

        builder
            .Property(operationLocation => operationLocation.UpdatedAt)
            .IsRequired(false);

        builder
            .Property(operationLocation => operationLocation.DeletedAt)
            .IsRequired(false);

        builder
            .Property(operationLocation => operationLocation.Latitude)
            .IsRequired();

        builder
            .Property(operationLocation => operationLocation.Longitude)
            .IsRequired();

        builder
            .Property(operationLocation => operationLocation.Title)
            .HasMaxLength(200)
            .IsRequired(false);

        builder
            .Property(operationLocation => operationLocation.Description)
            .HasMaxLength(2000)
            .IsRequired(false);
    }
}