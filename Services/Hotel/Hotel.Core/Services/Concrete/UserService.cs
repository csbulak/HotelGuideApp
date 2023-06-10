using Hotel.Core.Contexts.Ef;
using Hotel.Core.Entities;
using Hotel.Core.Repositories.Ef.Concrete;
using Hotel.Core.Services.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shared.Dtos;
using Shared.Enums;
using Shared.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Hotel.Core.Dtos;

namespace Hotel.Core.Services.Concrete;

public class UserService : Repository<User>, IUserService
{
    public UserService(HotelDbContext dbContext) : base(dbContext)
    {
    }

    private readonly HotelDbContext _dbContext;
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _configuration;

    public UserService(HotelDbContext dbContext, ITokenService tokenService, IConfiguration configuration) : base(dbContext)
    {
        _dbContext = dbContext;
        _tokenService = tokenService;
        _configuration = configuration;
    }

    public async Task<Response<TokenDto>> Auth(string username, string password)
    {
        try
        {
            var user = await GetByPredicate(x => x.Email.Equals(username));
            if (!user.IsSuccessful)
            {
                return Response<TokenDto>.Fail(ErrorCodes.NotFound, HttpStatusCode.NotFound);
            }

            var isValidPass = user.Data != null && ValidPassword(user.Data, password);
            if (!isValidPass)
            {
                return Response<TokenDto>.Fail(ErrorCodes.BadRequest, HttpStatusCode.BadRequest);
            }

            if (user.Data == null)
                return Response<TokenDto>.Fail(ErrorCodes.BadRequest, HttpStatusCode.BadRequest);

            var authClaims = await AuthClaims(user.Data);

            var token = _tokenService.CreateToken(authClaims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.Data.RefreshToken = refreshToken;
            user.Data.RefreshTokenExpiryTime =
                DateTime.Now.AddDays(Convert.ToDouble(_configuration.GetSection("TokenOption.RefreshTokenExpiration").Value));

            await _dbContext.SaveChangesAsync();

            var tokenDto = new TokenDto
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                Expiration = token.ValidTo,
            };

            return !String.IsNullOrWhiteSpace(tokenDto.AccessToken)
                ? Response<TokenDto>.Success(tokenDto, HttpStatusCode.OK)
                : Response<TokenDto>.Fail(ErrorCodes.BadRequest, HttpStatusCode.BadGateway);

        }
        catch (Exception)
        {
            return Response<TokenDto>.Fail(ErrorCodes.InternalServerError, HttpStatusCode.InternalServerError);
        }
    }

    public bool ValidPassword(User user, string password)
    {
        try
        {
            var hashPass = Helper.HashSha256(password);
            return user.PasswordHash.Equals(hashPass);
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<List<Claim>> AuthClaims(User user)
    {
        #region Find User Roles
        //var userRoles = await _context.Database.ExecuteSqlRawAsync("EXECUTE usp_UserRole @UserID", parameters: new[] { user.UserId });
        var userRoles = await GetRoleListByUserId(user.Id);
        #endregion

        var key = Encoding.ASCII.GetBytes(_configuration.GetSection("TokenOption:SecurityKey").Value ?? string.Empty);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new List<Claim>()),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var authClaims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        if (userRoles.Data != null)
            authClaims.AddRange(userRoles.Data.Select(userRole =>
                new Claim(ClaimTypes.Role, userRole.RoleName)));

        //tokenDescriptor.Subject.Claims.ToList().AddRange(authClaims);
        tokenDescriptor.Subject.AddClaims(authClaims);
        return authClaims;
    }

    public async Task<Response<List<RoleModel>>> GetRoleListByUserId(Guid userId)
    {
        List<RoleModel> model = new List<RoleModel>();
        var userRole = await
            _dbContext.UserRoles.Include(x => x.RoleGroup).Where(u => u.UserId.Equals(userId)).AsNoTracking().ToListAsync();

        if (!userRole.Any())
        {
            return Response<List<RoleModel>>.Fail(ErrorCodes.NotFound, HttpStatusCode.NotFound);
        }

        foreach (var item in userRole)
        {
            var allRoles = await _dbContext.Roles.Where(x => x.BitwiseId == (item.Roles & x.BitwiseId) && x.GroupId.Equals(item.RoleGroupId)).AsNoTracking().ToListAsync();
            allRoles.ForEach(x =>
            {
                model.Add(new RoleModel()
                {
                    RoleName = x.RoleName,
                    RoleGroupId = (int)x.GroupId,
                    RoleId = (int)x.BitwiseId,
                    UserId = userId,
                    GroupName = x.RoleGroup?.GroupName
                });
            });


        }


        return Response<List<RoleModel>>.Success(model);
    }

    public async Task<Response<bool>> LogOut(Guid userId)
    {
        try
        {
            var user = await GetByIdAsync(userId);
            if (!user.IsSuccessful) return Response<bool>.Fail(ErrorCodes.NotFound, HttpStatusCode.NotFound);
            if (user.Data != null)
            {
                user.Data.RefreshToken = null;
                user.Data.RefreshTokenExpiryTime = DateTime.Now;
            }

            await _dbContext.SaveChangesAsync();
            return Response<bool>.Success(true);
        }
        catch (Exception e)
        {
            return Response<bool>.Fail(ErrorCodes.InternalServerError, HttpStatusCode.InternalServerError);
        }
    }

    public async Task<Response<TokenDto>> RefreshToken(string refreshToken)
    {
        try
        {
            var user = await GetByPredicate(x => x.RefreshToken!.Equals(refreshToken));
            if (!user.IsSuccessful)
            {
                return Response<TokenDto>.Fail(ErrorCodes.BadRequest, HttpStatusCode.BadRequest);
            }

            var newAccessToken = _tokenService.CreateToken(await AuthClaims(user.Data ?? throw new InvalidOperationException()));
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.Data.RefreshToken = newRefreshToken;
            await _dbContext.SaveChangesAsync();

            var tokenDto = new TokenDto()
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                RefreshToken = newRefreshToken,
                Expiration = newAccessToken.ValidTo
            };

            return Response<TokenDto>.Success(tokenDto);
        }
        catch (Exception e)
        {
            return Response<TokenDto>.Fail(ErrorCodes.InternalServerError, HttpStatusCode.InternalServerError);
        }
    }

    public async Task<Response<User>> CreateUser(CreateUserDto user)
    {
        try
        {
            var isExistUser = _dbContext.Users.Any(x => x.Email.Equals(user.Email));
            if (isExistUser)
            {
                return Response<User>.Fail(ErrorCodes.BadRequest, HttpStatusCode.BadRequest);
            }

            var newUser = new User()
            {
                Email = user.Email,
                PasswordHash = Helper.HashSha256(user.Password)
            };
            //user.PasswordHash = Helper.HashSha256(user.PasswordHash);
            return await AddAsync(newUser);
        }
        catch (Exception e)
        {
            return Response<User>.Fail(ErrorCodes.InternalServerError, HttpStatusCode.InternalServerError);
        }
    }
}