using Hotel.Core.Contexts.Ef;
using Hotel.Core.Entities;
using Hotel.Core.Repositories.Ef.Concrete;
using Hotel.Core.Services.Abstract;

namespace Hotel.Core.Services.Concrete;

public class ContactService : Repository<Contact>, IContactService
{
    public ContactService(HotelDbContext dbContext) : base(dbContext)
    {
    }
}