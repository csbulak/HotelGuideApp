namespace Hotel.Core.Entities;

public class Hotel : BaseEntity
{
    public string AuthorizedName { get; set; }
    public string AuthorizedSurname { get; set; }
    public string CompanyTitle { get; set; }

    public virtual ICollection<Contact> Contacts { get; set; }
}