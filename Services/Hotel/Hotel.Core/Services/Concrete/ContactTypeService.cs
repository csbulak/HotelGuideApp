using Hotel.Core.Contexts.Ef;
using Hotel.Core.Entities;
using Hotel.Core.Repositories.Ef.Concrete;
using Hotel.Core.Services.Abstract;

namespace Hotel.Core.Services.Concrete;

public class ContactTypeService : Repository<ContactType>, IContactTypeService
{
    public ContactTypeService(HotelDbContext dbContext) : base(dbContext)
    {
    }
}