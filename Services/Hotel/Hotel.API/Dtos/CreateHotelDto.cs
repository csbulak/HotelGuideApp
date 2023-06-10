namespace Hotel.API.Dtos;

public class CreateHotelDto
{
    public string AuthorizedName { get; set; }
    public string AuthorizedSurname { get; set; }
    public string CompanyTitle { get; set; }

    public List<CreateContactDto> Contacts { get; set; }
}