namespace Hotel.Core.Entities;

public class UserRole : BaseEntity
{
    public Guid? UserId { get; set; }
    public int? RoleGroupId { get; set; }
    public RoleGroup? RoleGroup { get; set; }
    public long? Roles { get; set; }
}