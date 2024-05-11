using AutoMapper;
using Finder.Domain.Models.Views;

namespace Finder.Domain.Mapper.Converters.ContactInfo;

internal class ContactInfoToContactInfoViewConverter : ITypeConverter<Data.Entities.ContactInfo, ContactInfoView>
{
    public ContactInfoView Convert(
        Data.Entities.ContactInfo contactInfo,
        ContactInfoView contactInfoView,
        ResolutionContext context
    ) => new(
        contactInfo.PhoneNumber,
        contactInfo.Telegram,
        contactInfo.Skype,
        contactInfo.LinkedIn,
        contactInfo.Instagram,
        contactInfo.Other
    );
}