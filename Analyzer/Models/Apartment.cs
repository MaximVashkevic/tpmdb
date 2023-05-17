namespace Analyzer.Models;

public class Apartment
{
    public int Id { get; set; }
    public Price Price { get; set; }
    public string RentType { get; set; }
    public Location Location { get; set; }
    public string Photo { get; set; }
    public Contact Contact { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastTimeUp { get; set; }
    public string Url { get; set; }
}
