using System.Net;
using Hotel.Core.Contexts.Ef;
using Hotel.Core.Dtos;
using Hotel.Core.Entities;
using Hotel.Core.Repositories.Ef.Concrete;
using Hotel.Core.Services.Abstract;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;
using Shared.Enums;

namespace Hotel.Core.Services.Concrete;

public class UserRoleService : Repository<UserRole>, IUserRoleService
{
    private readonly HotelDbContext _dbContext;

    public UserRoleService(HotelDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<Response<CreateUserRoleDto>> CreateUserRole(CreateUserRoleDto createUserRole)
    {
        try
        {
            var role = await _dbContext.Roles.FirstOrDefaultAsync(x =>
                x.RoleId.Equals(createUserRole.RoleId) && x.GroupId.Equals(createUserRole.RoleGroupId));
            if (role == null)
            {
                return Response<CreateUserRoleDto>.Fail(ErrorCodes.NotFound, HttpStatusCode.NotFound);
            }

            var userRole = await _dbContext.UserRoles.FirstOrDefaultAsync(x =>
                x.UserId.Equals(createUserRole.UserId) && x.RoleGroupId.Equals(createUserRole.RoleGroupId));

            if (userRole != null)
            {
                if (role.BitwiseId != (userRole.Roles & role.BitwiseId))
                {
                    userRole.Roles += role.BitwiseId;
                    _dbContext.Update(userRole);
                }
                else
                {
                    return Response<CreateUserRoleDto>.Fail(ErrorCodes.BadRequest, HttpStatusCode.BadRequest);
                }
            }
            else
            {
                var newUserRole = new UserRole()
                {
                    UserId = createUserRole.UserId,
                    RoleGroupId = createUserRole.RoleGroupId,
                    Roles = role.BitwiseId
                };
                await _dbContext.AddAsync(newUserRole);
            }

            await _dbContext.SaveChangesAsync();
            return Response<CreateUserRoleDto>.Success(createUserRole);
        }
        catch (Exception)
        {
            return Response<CreateUserRoleDto>.Fail(ErrorCodes.InternalServerError, HttpStatusCode.InternalServerError);
        }
    }

    public async Task<Response<CreateUserRoleDto>> DeleteUserRole(CreateUserRoleDto deleteUserRoleDto)
    {
        try
        {
            var role = await _dbContext.Roles.FirstOrDefaultAsync(x =>
                x.RoleId.Equals(deleteUserRoleDto.RoleId) && x.GroupId.Equals(deleteUserRoleDto.RoleGroupId));
            if (role == null)
            {
                return Response<CreateUserRoleDto>.Fail(ErrorCodes.NotFound, HttpStatusCode.NotFound);
            }

            var userRole = await _dbContext.UserRoles.FirstOrDefaultAsync(x =>
                x.UserId.Equals(deleteUserRoleDto.UserId) && x.RoleGroupId.Equals(deleteUserRoleDto.RoleGroupId));
            if (userRole == null)
            {
                return Response<CreateUserRoleDto>.Fail(ErrorCodes.NotFound, HttpStatusCode.NotFound);
            }

            if (role.BitwiseId != (userRole.Roles & role.BitwiseId))
                return Response<CreateUserRoleDto>.Fail(ErrorCodes.NotFound, HttpStatusCode.NotFound);

            userRole.Roles -= role.BitwiseId;
            if (userRole.Roles == 0)
            {
                var removeResponse = await RemoveAsync(userRole.Id);
                return removeResponse.IsSuccessful
                    ? Response<CreateUserRoleDto>.Success(deleteUserRoleDto)
                    : Response<CreateUserRoleDto>.Fail(removeResponse.Errors, removeResponse.StatusCode);
            }

            var updateResponse = await UpdateAsync(userRole);
            return updateResponse.IsSuccessful
                ? Response<CreateUserRoleDto>.Success(deleteUserRoleDto)
                : Response<CreateUserRoleDto>.Fail(updateResponse.Errors, updateResponse.StatusCode);

        }
        catch (Exception)
        {
            return Response<CreateUserRoleDto>.Fail(ErrorCodes.InternalServerError, HttpStatusCode.InternalServerError);
        }
    }
}