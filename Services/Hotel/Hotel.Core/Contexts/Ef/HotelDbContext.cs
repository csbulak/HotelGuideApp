using Hotel.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Hotel.Core.Contexts.Ef;

public partial class HotelDbContext : DbContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HotelDbContext(DbContextOptions<HotelDbContext> options, IHttpContextAccessor httpContextAccessor)
        : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public DbSet<Contact>? Contacts { get; set; }
    public DbSet<ContactType>? ContactTypes { get; set; }
    public DbSet<Entities.Hotel>? Hotels { get; set; }
    public DbSet<Role>? Roles { get; set; }
    public DbSet<RoleGroup>? RoleGroups { get; set; }
    public DbSet<User>? Users { get; set; }
    public DbSet<UserRole>? UserRoles { get; set; }
}

