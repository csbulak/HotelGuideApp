using Hotel.Core.Entities;

namespace Hotel.Core.Dtos;

public class ContactDto
{
    public ContactTypeDto ContactType { get; set; }
    public string ContactText { get; set; }
}