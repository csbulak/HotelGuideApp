using AutoMapper;
using Hotel.Core.Dtos;
using Hotel.Core.Entities;

namespace Hotel.Core.Mapping;

public class CoreMapping : Profile
{
    public CoreMapping()
    {
        CreateMap<HotelDto, Entities.Hotel>().ReverseMap();
        CreateMap<Contact, ContactDto>().ReverseMap();
        CreateMap<ContactType, ContactTypeDto>().ReverseMap();
    }
}