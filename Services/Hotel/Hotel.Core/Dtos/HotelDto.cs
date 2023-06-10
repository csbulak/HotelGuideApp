namespace Hotel.Core.Dtos;

public class HotelDto
{
    public Guid Id { get; set; }
    public string AuthorizedName { get; set; }
    public string AuthorizedSurname { get; set; }
    public string CompanyTitle { get; set; }

    public List<ContactDto> Contacts { get; set; }
}