namespace Analyzer.Models;

public class Price
{
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public Dictionary<string, CurrencyData> Converted { get; set; }
}
