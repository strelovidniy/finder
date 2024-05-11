using Finder.Data.Entities;
using Finder.Data.Enums.RichEnums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Finder.Data.EntityConfigurations;

internal class ContactInfoConfiguration : IEntityTypeConfiguration<ContactInfo>
{
    public void Configure(EntityTypeBuilder<ContactInfo> builder)
    {
        builder
            .ToTable(TableName.ContactInfos, TableSchema.Dbo);

        builder
            .HasKey(contactInfo => contactInfo.Id);

        builder
            .Property(contactInfo => contactInfo.Id)
            .HasDefaultValueSql(DefaultSqlValue.NewGuid);

        builder
            .Property(contactInfo => contactInfo.CreatedAt)
            .HasDefaultValueSql(DefaultSqlValue.NowUtc);

        builder
            .Property(contactInfo => contactInfo.UpdatedAt)
            .IsRequired(false);

        builder
            .Property(contactInfo => contactInfo.DeletedAt)
            .IsRequired(false);

        builder
            .Property(contactInfo => contactInfo.Instagram)
            .IsRequired(false)
            .HasMaxLength(200);

        builder
            .Property(contactInfo => contactInfo.LinkedIn)
            .IsRequired(false)
            .HasMaxLength(200);

        builder
            .Property(contactInfo => contactInfo.PhoneNumber)
            .IsRequired(false)
            .HasMaxLength(100);

        builder
            .Property(contactInfo => contactInfo.Skype)
            .IsRequired(false)
            .HasMaxLength(200);

        builder
            .Property(contactInfo => contactInfo.Telegram)
            .IsRequired(false)
            .HasMaxLength(200);

        builder
            .Property(contactInfo => contactInfo.Other)
            .IsRequired(false)
            .HasMaxLength(1000);
    }
}