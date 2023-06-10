using Hotel.Core.Contexts.Ef;
using Hotel.Core.Entities;
using Hotel.Core.Repositories.Ef.Concrete;
using Hotel.Core.Services.Abstract;

namespace Hotel.Core.Services.Concrete;

public class RoleGroupService : Repository<RoleGroup>, IRoleGroupService
{
    public RoleGroupService(HotelDbContext dbContext) : base(dbContext)
    {
    }
}