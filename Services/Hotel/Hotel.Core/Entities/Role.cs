using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel.Core.Entities;

public class Role
{
    public int RoleId { get; set; }

    public long? BitwiseId { get; set; }

    public string RoleName { get; set; } = null!;

    [ForeignKey("RoleGroup")]
    public int? GroupId { get; set; }
    public RoleGroup? RoleGroup { get; set; }
}