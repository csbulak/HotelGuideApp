using System.Net;
using AutoMapper;
using Hotel.Core.Contexts.Ef;
using Hotel.Core.Dtos;
using Hotel.Core.Repositories.Ef.Concrete;
using Hotel.Core.Services.Abstract;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;
using Shared.Enums;

namespace Hotel.Core.Services.Concrete;

public class HotelService : Repository<Entities.Hotel>, IHotelService
{
    private readonly HotelDbContext _context;
    private readonly IMapper _mapper;
    public HotelService(HotelDbContext dbContext, HotelDbContext context, IMapper mapper) : base(dbContext)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Response<bool>> RemoveHotel(Guid hotelId)
    {
        var hotel = await _context.Hotels.Include(x => x.Contacts).FirstOrDefaultAsync(x => x.Id.Equals(hotelId));
        if (hotel != null)
        {
            hotel.IsDeleted = true;
            hotel.IsActive = false;
            await _context.SaveChangesAsync();
            return Response<bool>.Success(true);
        }
        return Response<bool>.Fail(ErrorCodes.NotFound, HttpStatusCode.NotFound);
    }

    public async Task<Response<HotelDto>> GetHotelById(Guid hotelId)
    {
        try
        {
            var hotel = await _context.Hotels.Include(x => x.Contacts).ThenInclude(x => x.ContactType)
                .Select(x => new HotelDto()
                {
                    Contacts = _mapper.Map<List<ContactDto>>(x.Contacts),
                    AuthorizedName = x.AuthorizedName,
                    AuthorizedSurname = x.AuthorizedSurname,
                    CompanyTitle = x.CompanyTitle,
                    Id = x.Id
                })
                
                .FirstOrDefaultAsync(x => x.Id.Equals(hotelId));

            return hotel != null ? Response<HotelDto>.Success(hotel) : Response<HotelDto>.Fail(ErrorCodes.NotFound, HttpStatusCode.NotFound);
        }
        catch (Exception e)
        {
            return Response<HotelDto>.Fail(new List<ErrorDto>() { new ErrorDto(ErrorCodes.InternalServerError) { ErrorCode = ErrorCodes.InternalServerError, ErrorMessage = e.Message } }, HttpStatusCode.InternalServerError);
        }
    }
}