using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Hotel.Core.Services.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Hotel.Core.Services.Concrete;

public class TokenService : ITokenService
{
    private readonly ISignService _signService;
    private readonly IConfiguration _configuration;
    public TokenService(ISignService signService, IConfiguration configuration)
    {
        _signService = signService;
        _configuration = configuration;
    }

    public JwtSecurityToken CreateToken(IEnumerable<Claim> authClaims)
    {
        var authSigningKey = _signService.GetSymmetricSecurityKey(
            _configuration.GetSection("TokenOption:SecurityKey").Value ?? string.Empty);

        var token = new JwtSecurityToken(
            issuer: _configuration.GetSection("TokenOption:Issuer").Value,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration.GetSection("TokenOption:AccessTokenExpiration").Value)),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        String token = Convert.ToHexString(randomNumber);
        return token;
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("TokenOption:SecurityKey").Value ?? throw new InvalidOperationException())),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return principal;
    }
}