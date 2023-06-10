using Hotel.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Hotel.Core.Contexts.Ef;

public partial class HotelDbContext
{
    private void OnBeforeSaving()
    {
        var entries = ChangeTracker.Entries();
        if (_httpContextAccessor.HttpContext != null)
        {
            var userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            foreach (var entry in entries)
            {
                switch (entry.Entity)
                {
                    case BaseEntity tractable:
                        {
                            var now = DateTime.Now;
                            switch (entry.State)
                            {
                                case EntityState.Added:
                                    tractable.Id = Guid.NewGuid();
                                    tractable.CreateDate = now;
                                    tractable.CreateBy = Guid.Parse(userId ?? throw new InvalidOperationException()); // Auth mekanizması aktif olunca bu satır etkinleştirilebilir.
                                    tractable.IsActive = true;
                                    tractable.IsDeleted = false;
                                    break;

                                case EntityState.Modified:
                                    tractable.UpdateDate = now;
                                    tractable.UpdateBy = Guid.Parse(userId ?? throw new InvalidOperationException()); // Auth mekanizması aktif olunca bu satır etkinleştirilebilir.
                                    break;

                                case EntityState.Detached:
                                case EntityState.Unchanged:
                                case EntityState.Deleted:
                                    tractable.IsActive = false;
                                    tractable.IsDeleted = true;
                                    tractable.UpdateDate = now;
                                    tractable.UpdateBy = Guid.Parse(userId ?? throw new InvalidOperationException());
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }

                            break;
                        }
                }
            }
        }

    }



    public override int SaveChanges()
    {
        OnBeforeSaving();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        OnBeforeSaving();
        return base.SaveChangesAsync(cancellationToken);
    }
}