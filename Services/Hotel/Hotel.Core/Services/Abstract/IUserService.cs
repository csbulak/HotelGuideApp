using Hotel.Core.Entities;
using Hotel.Core.Repositories.Ef.Abstract;
using Shared.Dtos;
using System.Security.Claims;
using Hotel.Core.Dtos;

namespace Hotel.Core.Services.Abstract;

public interface IUserService : IRepository<User>
{
    Task<Response<TokenDto>> Auth(string username, string password);
    bool ValidPassword(User user, string password);
    Task<List<Claim>> AuthClaims(User user);
    Task<Response<List<RoleModel>>> GetRoleListByUserId(Guid userId);
    Task<Response<Boolean>> LogOut(Guid userId);
    Task<Response<TokenDto>> RefreshToken(string refreshToken);
    Task<Response<User>> CreateUser(CreateUserDto user);
}