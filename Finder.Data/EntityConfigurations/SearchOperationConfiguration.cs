using Finder.Data.Entities;
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
            .ToTable(TableName.HelpRequests, TableSchema.Dbo);

        builder
            .HasKey(helpRequest => helpRequest.Id);

        builder
            .Property(helpRequest => helpRequest.Id)
            .HasDefaultValueSql(DefaultSqlValue.NewGuid);

        builder
            .Property(helpRequest => helpRequest.CreatedAt)
            .HasDefaultValueSql(DefaultSqlValue.NowUtc);

        builder
            .Property(helpRequest => helpRequest.UpdatedAt)
            .IsRequired(false);

        builder
            .Property(helpRequest => helpRequest.DeletedAt)
            .IsRequired(false);

        builder
            .Property(helpRequest => helpRequest.UserId)
            .IsRequired();

        builder
            .Property(helpRequest => helpRequest.Tags)
            .HasConversion(
                tags => JsonConvert.SerializeObject(tags),
                json => JsonConvert.DeserializeObject<List<string>>(json) ?? Enumerable.Empty<string>()
            )
            .IsRequired();

        builder
            .Property(helpRequest => helpRequest.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder
            .Property(helpRequest => helpRequest.Description)
            .HasMaxLength(2000)
            .IsRequired();

        builder
            .Property(helpRequest => helpRequest.ShowContactInfo)
            .IsRequired();

        builder
            .HasMany(helpRequest => helpRequest.Images)
            .WithOne()
            .HasForeignKey(image => image.OperationId)
            .HasPrincipalKey(helpRequest => helpRequest.Id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}