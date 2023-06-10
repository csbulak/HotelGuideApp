using Hotel.Core.Dtos;
using Hotel.Core.Entities;
using Hotel.Core.Repositories.Ef.Abstract;
using Shared.Dtos;

namespace Hotel.Core.Services.Abstract;

public interface IRoleService : IRepository<Role>
{
    public Task<Response<RoleModel>> GetRoleById(Guid userId, int roleGroupID, long roleID);
    public Response<RoleModel> GetRoleListByGroupId(Guid userId, int roleGroupID);
    public Task<Response<CreateRoleDto>> CreateRole(CreateRoleDto role);
}