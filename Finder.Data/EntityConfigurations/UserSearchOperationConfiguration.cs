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
            .HasKey(userSearchOperation => userSearchOperation.Id);

        builder
            .Property(userSearchOperation => userSearchOperation.Id)
            .HasDefaultValueSql(DefaultSqlValue.NewGuid);

        builder
            .Property(userSearchOperation => userSearchOperation.CreatedAt)
            .HasDefaultValueSql(DefaultSqlValue.NowUtc);

        builder
            .Property(userSearchOperation => userSearchOperation.UpdatedAt)
            .IsRequired(false);

        builder
            .Property(userSearchOperation => userSearchOperation.DeletedAt)
            .IsRequired(false);

        builder
            .HasKey(userSearchOperation => new
            {
                userSearchOperation.UserId,
                userSearchOperation.SearchOperationId
            });

        builder
            .HasOne(userSearchOperation => userSearchOperation.User)
            .WithMany(user => user.UserSearchOperations)
            .HasForeignKey(userSearchOperation => userSearchOperation.UserId);

        builder
            .HasOne(userSearchOperation => userSearchOperation.SearchOperation)
            .WithMany(searchOperation => searchOperation.UserApplications)
            .HasForeignKey(userSearchOperation => userSearchOperation.SearchOperationId);
    }
}