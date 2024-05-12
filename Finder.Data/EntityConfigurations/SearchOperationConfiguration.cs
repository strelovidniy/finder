using Finder.Data.Entities;
using Finder.Data.Enums;
using Finder.Data.Enums.RichEnums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace Finder.Data.EntityConfigurations;

internal class SearchOperationConfiguration : IEntityTypeConfiguration<SearchOperation>
{
    public void Configure(EntityTypeBuilder<SearchOperation> builder)
    {
        builder
            .ToTable(TableName.SearchOperation, TableSchema.Dbo);

        builder
            .HasKey(searchOperation => searchOperation.Id);

        builder
            .Property(searchOperation => searchOperation.Id)
            .HasDefaultValueSql(DefaultSqlValue.NewGuid);

        builder
            .Property(searchOperation => searchOperation.CreatedAt)
            .HasDefaultValueSql(DefaultSqlValue.NowUtc);

        builder
            .Property(searchOperation => searchOperation.UpdatedAt)
            .IsRequired(false);

        builder
            .Property(searchOperation => searchOperation.DeletedAt)
            .IsRequired(false);

        builder
            .Property(searchOperation => searchOperation.CreatorUserId)
            .IsRequired();

        builder
            .Property(searchOperation => searchOperation.Tags)
            .HasConversion(
                tags => JsonConvert.SerializeObject(tags),
                json => JsonConvert.DeserializeObject<List<string>>(json) ?? Enumerable.Empty<string>()
            )
            .IsRequired();

        builder
            .Property(searchOperation => searchOperation.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder
            .Property(searchOperation => searchOperation.Description)
            .HasMaxLength(2000)
            .IsRequired();

        builder
            .Property(searchOperation => searchOperation.ChatLink)
            .HasMaxLength(2000)
            .IsRequired(false);

        builder
            .Property(searchOperation => searchOperation.ShowContactInfo)
            .IsRequired();

        builder
            .Property(searchOperation => searchOperation.OperationType)
            .IsRequired();

        builder
            .Property(searchOperation => searchOperation.OperationStatus)
            .HasDefaultValue(SearchOperationStatus.Pending)
            .IsRequired();

        builder
            .HasMany(searchOperation => searchOperation.Images)
            .WithOne()
            .HasForeignKey(image => image.OperationId)
            .HasPrincipalKey(searchOperation => searchOperation.Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(s => s.OperationLocations)
            .WithOne(l => l.SearchOperation)
            .HasForeignKey(l => l.SearchOperationId);
    }
}