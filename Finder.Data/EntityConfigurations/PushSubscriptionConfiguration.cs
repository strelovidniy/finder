using Finder.Data.Entities;
using Finder.Data.Enums.RichEnums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Finder.Data.EntityConfigurations;

internal class PushSubscriptionConfiguration : IEntityTypeConfiguration<PushSubscription>
{
    public void Configure(EntityTypeBuilder<PushSubscription> builder)
    {
        builder
            .ToTable(TableName.PushSubscriptions, TableSchema.Dbo);

        builder
            .HasKey(pushSubscription => pushSubscription.Id);

        builder
            .Property(pushSubscription => pushSubscription.Id)
            .HasDefaultValueSql(DefaultSqlValue.NewGuid);

        builder
            .Property(pushSubscription => pushSubscription.CreatedAt)
            .HasDefaultValueSql(DefaultSqlValue.NowUtc);

        builder
            .Property(pushSubscription => pushSubscription.UpdatedAt)
            .IsRequired(false);

        builder
            .Property(pushSubscription => pushSubscription.DeletedAt)
            .IsRequired(false);

        builder
            .Property(pushSubscription => pushSubscription.UserId)
            .IsRequired();

        builder
            .Property(pushSubscription => pushSubscription.Auth)
            .HasMaxLength(500)
            .IsRequired();

        builder
            .Property(pushSubscription => pushSubscription.Endpoint)
            .HasMaxLength(500)
            .IsRequired();

        builder
            .Property(pushSubscription => pushSubscription.P256dh)
            .HasMaxLength(500)
            .IsRequired();

        builder
            .Property(pushSubscription => pushSubscription.ExpirationTime)
            .IsRequired(false);
    }
}