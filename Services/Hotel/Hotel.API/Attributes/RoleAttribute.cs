namespace Hotel.API.Attributes;

[AttributeUsage(AttributeTargets.All)]
public class RoleAttribute : Attribute
{
    public RoleAttribute(int roleGroupId, long roleId)
    {
        this.RoleGroupId = roleGroupId;
        this.RoleId = roleId;
    }

    public int RoleGroupId { get; }
    public long RoleId { get; }
}