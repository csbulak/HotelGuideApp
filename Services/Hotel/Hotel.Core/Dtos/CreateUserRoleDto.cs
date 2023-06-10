namespace Hotel.Core.Dtos;

public class CreateUserRoleDto
{
    public Guid UserId { get; set; }
    public int RoleGroupId { get; set; }
    public int RoleId { get; set; }
}