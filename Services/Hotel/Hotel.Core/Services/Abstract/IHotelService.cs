using Hotel.Core.Dtos;
using Hotel.Core.Repositories.Ef.Abstract;
using Shared.Dtos;

namespace Hotel.Core.Services.Abstract;

public interface IHotelService : IRepository<Entities.Hotel>
{
    public Task<Response<bool>> RemoveHotel(Guid hotelId);
    public Task<Response<HotelDto>> GetHotelById(Guid hotelId);
}