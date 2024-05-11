using AutoMapper;
using Finder.Data.Entities;
using Finder.Domain.Mapper.Converters.ContactInfo;
using Finder.Domain.Models.Views;

namespace Finder.Domain.Mapper.Profiles;

internal class ContactInfoMapperProfile : Profile
{
    public ContactInfoMapperProfile()
    {
        CreateMap<ContactInfo, ContactInfoView>().ConvertUsing(new ContactInfoToContactInfoViewConverter());
    }
}