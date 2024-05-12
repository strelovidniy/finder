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
            .HasKey(userDetails => userDetails.Id);

        builder
            .Property(userDetails => userDetails.Id)
            .HasDefaultValueSql(DefaultSqlValue.NewGuid);

        builder
            .Property(userDetails => userDetails.CreatedAt)
            .HasDefaultValueSql(DefaultSqlValue.NowUtc);

        builder
            .Property(userDetails => userDetails.UpdatedAt)
            .IsRequired(false);

        builder
            .Property(userDetails => userDetails.DeletedAt)
            .IsRequired(false);
        
        builder
            .Property(helpRequest => helpRequest.Latitude)
            .IsRequired(true);

        builder
            .Property(helpRequest => helpRequest.Longitude)
            .IsRequired(true);
        
        builder
            .Property(searchOperation => searchOperation.Title)
            .HasMaxLength(200)
            .IsRequired(false);

        builder
            .Property(searchOperation => searchOperation.Description)
            .HasMaxLength(2000)
            .IsRequired(false);
    }
}