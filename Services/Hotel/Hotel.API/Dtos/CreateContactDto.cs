using Hotel.Core.Entities;

namespace Hotel.API.Dtos;

public class CreateContactDto
{
    public Guid ContactTypeId { get; set; }
    public Guid HotelId { get; set; }
    public string ContactText { get; set; }
}