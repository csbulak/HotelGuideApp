using Microsoft.IdentityModel.Tokens;

namespace Hotel.Core.Services.Abstract;

public interface ISignService
{
    public SecurityKey GetSymmetricSecurityKey(String securityKey);
}