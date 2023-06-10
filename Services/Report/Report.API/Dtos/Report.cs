namespace Report.API.Dtos;

public class Report
{
    public string UUID { get; set; }
    public DateTime RequestedDate { get; set; }
    public string Status { get; set; }
}