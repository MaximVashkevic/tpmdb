using System.Net.Http.Formatting;
using Analyzer;
using Analyzer.DataAnalyzer;
using Analyzer.Models;
using Analyzer.Scraper;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

var scraper = new ApiScraper<ApartmentApiResponse, Apartment>(r => r.Apartments, new[]
{
    new JsonMediaTypeFormatter
    {
        SerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            }
        }
    }
}, new HttpClient());
const string fileName = "apartments.csv";
var csvWriter = new CsvWriter(fileName);

await csvWriter.WriteRecordsAsync(
    scraper.ScrapeAsync("https://ak.api.onliner.by/search/apartments?currency=usd"));

var csvReader = new CsvReader<Apartment>(fileName);
await DataAnalyzer.AnalyzeAsync(csvReader);
