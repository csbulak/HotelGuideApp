using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Hotel.Core.Services.Abstract;

public interface ITokenService
{
    public JwtSecurityToken CreateToken(IEnumerable<Claim> authClaims);

    public String GenerateRefreshToken();

    public ClaimsPrincipal GetPrincipalFromExpiredToken(String token);
}