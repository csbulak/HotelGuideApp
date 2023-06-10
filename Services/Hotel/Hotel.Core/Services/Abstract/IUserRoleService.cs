using Hotel.Core.Dtos;
using Hotel.Core.Entities;
using Hotel.Core.Repositories.Ef.Abstract;
using Shared.Dtos;

namespace Hotel.Core.Services.Abstract;

public interface IUserRoleService : IRepository<UserRole>
{
    Task<Response<CreateUserRoleDto>> CreateUserRole(CreateUserRoleDto createUserRoleDto);
    Task<Response<CreateUserRoleDto>> DeleteUserRole(CreateUserRoleDto deleteUserRoleDto);
}