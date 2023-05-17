using Analyzer.Models;
using Analyzer.Scraper;

namespace Analyzer;

public class ApartmentApiResponse : ApiResponse<Apartment>
{
    public List<Apartment> Apartments { get; set; }
}
