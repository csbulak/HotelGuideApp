using System.Text;
using Hotel.Core.Contexts.Ef;
using Hotel.Core.Mapping;
using Hotel.Core.Repositories.Ef.Abstract;
using Hotel.Core.Repositories.Ef.Concrete;
using Hotel.Core.Services.Abstract;
using Hotel.Core.Services.Concrete;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Hotel.Core;

public static class ServiceIntegration
{
    public static IServiceCollection AddCoreRegister(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("HotelDb") ?? String.Empty;
        var tokenSecurityKey = configuration.GetSection("TokenOption:SecurityKey")?.Value ?? String.Empty;

        services.AddDbContext<HotelDbContext>(options => options.UseSqlServer(connectionString));

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSecurityKey))
            };
        });

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IContactTypeService, ContactTypeService>();
        services.AddScoped<IContactService, ContactService>();
        services.AddScoped<IHotelService, HotelService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IRoleGroupService, RoleGroupService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserRoleService, UserRoleService>();
        services.AddScoped<ISignService, SignService>();
        services.AddScoped<ITokenService, TokenService>();

        services.AddAutoMapper(typeof(CoreMapping));


        return services;
    }
}