using Finder.Data.Entities;
using Finder.Data.Enums;
using Finder.Data.Enums.RichEnums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Finder.Data.EntityConfigurations;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .ToTable(TableName.Users, TableSchema.Dbo);

        builder
            .HasKey(user => user.Id);

        builder
            .Property(user => user.Id)
            .HasDefaultValueSql(DefaultSqlValue.NewGuid);

        builder
            .Property(user => user.CreatedAt)
            .HasDefaultValueSql(DefaultSqlValue.NowUtc);

        builder
            .Property(user => user.UpdatedAt)
            .IsRequired(false);

        builder
            .Property(user => user.DeletedAt)
            .IsRequired(false);

        builder
            .Property(user => user.FirstName)
            .HasMaxLength(50);

        builder
            .Property(user => user.LastName)
            .HasMaxLength(50);

        builder
            .Property(user => user.Email)
            .IsRequired()
            .HasMaxLength(200);

        builder
            .Property(user => user.PasswordHash)
            .HasMaxLength(200);

        builder
            .Property(user => user.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .HasDefaultValue(UserStatus.Pending);

        builder
            .Property(user => user.RefreshToken)
            .HasMaxLength(100)
            .IsRequired(false);

        builder
            .Property(user => user.RefreshTokenExpiresAt)
            .IsRequired(false);

        builder
            .Property(user => user.RoleId)
            .IsRequired(false);

        builder
            .Ignore(user => user.FullName);

        builder
            .HasIndex(user => user.Email)
            .IsUnique();

        builder
            .HasOne(user => user.Role)
            .WithMany(role => role.Users)
            .HasForeignKey(user => user.RoleId)
            .HasPrincipalKey(role => role.Id)
            .OnDelete(DeleteBehavior.SetNull);

        builder
            .HasOne(user => user.Details)
            .WithOne(details => details.User)
            .HasForeignKey<UserDetails>(details => details.UserId)
            .HasPrincipalKey<User>(user => user.Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(user => user.PushSubscriptions)
            .WithOne(pushSubscription => pushSubscription.User)
            .HasForeignKey(pushSubscription => pushSubscription.UserId)
            .HasPrincipalKey(user => user.Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(user => user.NotificationSettings)
            .WithOne(notificationSettings => notificationSettings.User)
            .HasForeignKey<NotificationSettings>(notificationSettings => notificationSettings.UserId)
            .HasPrincipalKey<User>(user => user.Id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}