using Finder.Data.Entities;
using Finder.Data.Enums.RichEnums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Finder.Data.EntityConfigurations;

internal class UserSearchOperationConfiguration : IEntityTypeConfiguration<UserSearchOperation>
{
    public void Configure(EntityTypeBuilder<UserSearchOperation> builder)
    {
        builder
            .ToTable(TableName.UserSearchOperation, TableSchema.Dbo);

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
            .HasKey(us => new { us.UserId, us.SearchOperationId });

        builder
            .HasOne(us => us.User)
            .WithMany(u => u.UserSearchOperations)
            .HasForeignKey(us => us.UserId);

        builder
            .HasOne(us => us.SearchOperation)
            .WithMany(s => s.UserApplications)
            .HasForeignKey(us => us.SearchOperationId);
    }
}