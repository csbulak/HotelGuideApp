using System.Text;
using Hotel.Core.Services.Abstract;
using Microsoft.IdentityModel.Tokens;

namespace Hotel.Core.Services.Concrete;

public class SignService : ISignService
{
    public SecurityKey GetSymmetricSecurityKey(string securityKey)
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
    }
}