using AutoMapper;
using Hotel.API.Attributes;
using Hotel.API.Dtos;
using Hotel.API.Filters;
using Hotel.Core.Entities;
using Hotel.Core.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.BaseController;
using Shared.Enums;

namespace Hotel.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ServiceFilter(typeof(PermissionFilter))]
    public class HotelController : CustomBaseController
    {
        private readonly IHotelService _hotelService;
        private readonly IMapper _mapper;
        private readonly IContactTypeService _contactTypeService;
        private readonly IContactService _contactService;

        public HotelController(IHotelService hotelService, IMapper mapper, IContactTypeService contactTypeService, IContactService contactService)
        {
            _hotelService = hotelService;
            _mapper = mapper;
            _contactTypeService = contactTypeService;
            _contactService = contactService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateHotel([FromBody] CreateHotelDto createHotelDto)
        {
            return CreateActionResultInstance(await _hotelService.AddAsync(_mapper.Map<Core.Entities.Hotel>(createHotelDto)));
        }

        [HttpPost]
        public async Task<IActionResult> CreateContactType([FromBody] CreateContactTypeDto createContactTypeDto)
        {
            return CreateActionResultInstance(await _contactTypeService.AddAsync(_mapper.Map<ContactType>(createContactTypeDto)));
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveHotel(Guid hotelId)
        {
            return CreateActionResultInstance(await _hotelService.RemoveHotel(hotelId));
        }

        [HttpPost]
        public async Task<IActionResult> CreateContractByHotel([FromBody] CreateContactDto createContactDto)
        {
            return CreateActionResultInstance(await _contactService.AddAsync(_mapper.Map<Contact>(createContactDto)));
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveContractFromHotel(Guid contactId)
        {
            return CreateActionResultInstance(await _contactService.RemoveAsync(contactId));
        }

        [HttpGet]
        public async Task<IActionResult> GetHotelById(Guid hotelId)
        {
            return CreateActionResultInstance(await _hotelService.GetHotelById(hotelId));
        }

    }
}
