using AutoMapper;
using Hotel.API.Dtos;
using Hotel.Core.Dtos;
using Hotel.Core.Entities;

namespace Hotel.API.Mapping;

public class CustomMap : Profile
{
    public CustomMap()
    {
        CreateMap<CreateHotelDto, Core.Entities.Hotel>();
        CreateMap<CreateContactDto, Contact>();
        CreateMap<CreateContactTypeDto, ContactType>();

        CreateMap<CreateRoleDto, Role>();
        CreateMap<CreateRoleGroupDto, RoleGroup>();
        CreateMap<CreateUserDto, User>();
        
        
    }
}