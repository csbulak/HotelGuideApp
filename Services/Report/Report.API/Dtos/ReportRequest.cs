namespace Report.API.Dtos;

public class ReportRequest
{
    public string Location { get; set; }
    public int HotelCount { get; set; }
    public int PhoneNumberCount { get; set; }

    public override string ToString()
    {
        return $"Location: {Location}, HotelCount: {HotelCount}, PhoneNumberCount: {PhoneNumberCount}";
    }
}