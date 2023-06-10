using System.ComponentModel.DataAnnotations;

namespace Hotel.Core.Entities;

public class BaseEntity
{
    [Key]
    public Guid Id { get; set; }
    public DateTime? CreateDate { get; set; }

    public Guid? CreateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public Guid? UpdateBy { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }
}