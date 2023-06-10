namespace Shared.Dtos;

public class RoleModel
{
    public Guid UserId { get; set; }
    public int RoleGroupId { get; set; }
    public long RoleId { get; set; }
    public string? GroupName { get; set; }
    public string RoleName { get; set; }
}