namespace Hotel.Core.Entities;

public class ContactType : BaseEntity
{
    public string ContactTypeName { get; set; }

    public virtual ICollection<Contact> Contacts { get; set; }
}