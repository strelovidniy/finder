using Finder.Data.Entities;
using Finder.Data.Enums.RichEnums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace Finder.Data.EntityConfigurations;

internal class NotificationSettingsConfiguration : IEntityTypeConfiguration<NotificationSettings>
{
    public void Configure(EntityTypeBuilder<NotificationSettings> builder)
    {
        builder
            .ToTable(TableName.NotificationSettings, TableSchema.Dbo);

        builder
            .HasKey(notificationSetting => notificationSetting.Id);

        builder
            .Property(notificationSetting => notificationSetting.Id)
            .HasDefaultValueSql(DefaultSqlValue.NewGuid);

        builder
            .Property(notificationSetting => notificationSetting.CreatedAt)
            .HasDefaultValueSql(DefaultSqlValue.NowUtc);

        builder
            .Property(notificationSetting => notificationSetting.UpdatedAt)
            .IsRequired(false);

        builder
            .Property(notificationSetting => notificationSetting.DeletedAt)
            .IsRequired(false);

        builder
            .Property(notificationSetting => notificationSetting.FilterTags)
            .HasConversion(
                tags => tags != null
                    ? JsonConvert.SerializeObject(tags)
                    : null,
                json => json != null
                    ? JsonConvert.DeserializeObject<IEnumerable<string>>(json)
                    : null
            );

        builder
            .Property(notificationSetting => notificationSetting.FilterTitles)
            .HasConversion(
                titles => titles != null
                    ? JsonConvert.SerializeObject(titles)
                    : null,
                json => json != null
                    ? JsonConvert.DeserializeObject<IEnumerable<string>>(json)
                    : null
            );
    }
}