using AutoMapper;
using Finder.Data.Entities;
using Finder.Domain.Mapper.Converters.NotificationSettings;
using Finder.Domain.Models.Views;

namespace Finder.Domain.Mapper.Profiles;

internal class NotificationSettingsMapperProfile : Profile
{
    public NotificationSettingsMapperProfile()
    {
        CreateMap<NotificationSettings, NotificationSettingsView>()
            .ConvertUsing(new NotificationSettingsToNotificationSettingsViewConverter());
    }
}