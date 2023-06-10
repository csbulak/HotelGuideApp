namespace Hotel.Core.Entities;

public class Contact : BaseEntity
{
    public ContactType ContactType { get; set; }
    public Guid ContactTypeId { get; set; }

    public Hotel Hotel { get; set; }
    public Guid HotelId { get; set; }

    public string ContactText { get; set; }
}