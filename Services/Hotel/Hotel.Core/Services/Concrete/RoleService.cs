using Hotel.Core.Contexts.Ef;
using Hotel.Core.Dtos;
using Hotel.Core.Entities;
using Hotel.Core.Repositories.Ef.Concrete;
using Hotel.Core.Services.Abstract;
using Shared.Dtos;
using Shared.Enums;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Core.Services.Concrete;

public class RoleService : Repository<Role>, IRoleService
{

    private readonly HotelDbContext _dbContext;
    public RoleService(HotelDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Response<RoleModel>> GetRoleById(Guid userId, int roleGroupID, long roleID)
    {
        try
        {
            var userRole = await _dbContext.UserRoles.Include(x => x.RoleGroup)
                .FirstOrDefaultAsync(ur => ur.UserId.Equals(userId) && ur.RoleGroupId.Equals(roleGroupID));

            if (userRole == null) return Response<RoleModel>.Fail(ErrorCodes.NotFound, HttpStatusCode.NotFound);
            if (roleID != (userRole.Roles & roleID))
                return Response<RoleModel>.Fail(ErrorCodes.NotFound, HttpStatusCode.NotFound);
            var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.BitwiseId == roleID);
            return role != null
                ? Response<RoleModel>.Success(new RoleModel()
                {
                    RoleName = role.RoleName,
                    RoleGroupId = (int)userRole.RoleGroupId,
                    RoleId = roleID,
                    UserId = userId,
                    GroupName = userRole.RoleGroup.GroupName
                })
                : Response<RoleModel>.Fail(ErrorCodes.NotFound,
                    HttpStatusCode.NotFound);
        }
        catch (Exception e)
        {
            return Response<RoleModel>.Fail(ErrorCodes.InternalServerError, HttpStatusCode.InternalServerError);
        }
    }

    public Response<RoleModel> GetRoleListByGroupId(Guid userId, int roleGroupID)
    {
        throw new NotImplementedException();
    }

    public async Task<Response<CreateRoleDto>> CreateRole(CreateRoleDto role)
    {
        try
        {
            var existRole = await _dbContext.Roles.Where(x => x.GroupId.Equals(role.GroupId)).ToListAsync();
            if (existRole.Any())
            {
                var lastRoleInGroup = existRole.MaxBy(x => x.BitwiseId);
                var newRoleInGroup = new Role()
                {
                    BitwiseId = (lastRoleInGroup?.BitwiseId ?? 1) * 2,
                    RoleName = role.RoleName,
                    GroupId = role.GroupId
                };
                await _dbContext.AddAsync(newRoleInGroup);
            }
            else
            {
                var newFirstRoleInGroup = new Role()
                {
                    BitwiseId = 1,
                    RoleName = role.RoleName,
                    GroupId = role.GroupId
                };

                await _dbContext.AddAsync(newFirstRoleInGroup);
            }

            await _dbContext.SaveChangesAsync();
            return Response<CreateRoleDto>.Success(role);

        }
        catch (Exception e)
        {
            return Response<CreateRoleDto>.Fail(ErrorCodes.InternalServerError, HttpStatusCode.InternalServerError);
        }
    }
}