using System.Net;
using System.Net.Http.Formatting;
using Analyzer.Models;
using Analyzer.Scraper;

namespace Analyzer.Tests;

[TestFixture]
public class ApiScraperTests
{
    private const string TestApiUrl = "https://example.com/api?data=all";

    [Test]
    public async Task ScrapeAsync_ReturnsExpectedEntities()
    {
        // Arrange
        var expectedEntities = new List<Apartment>
        {
            new() {Id = 1, RentType = "Type1"},
            new() {Id = 2, RentType = "Type2"},
            new() {Id = 3, RentType = "Type1"}
        };

        var httpClient = new HttpClient(new TestHttpMessageHandler(expectedEntities, 3));
        var scraper =
            new ApiScraper<ApartmentApiResponse, Apartment>(response => response.Apartments, null, httpClient);

        // Act
        var entities = await scraper.ScrapeAsync(TestApiUrl).ToListAsync();

        // Assert
        CollectionAssert.AreEquivalent(expectedEntities, entities);
    }

    [Test]
    public async Task ScrapeAsync_WithPagination_ReturnsAllEntities()
    {
        // Arrange
        var expectedEntities = new List<Apartment>
        {
            new() {Id = 1, RentType = "Type1"},
            new() {Id = 2, RentType = "Type2"},
            new() {Id = 3, RentType = "Type1"},
            new() {Id = 4, RentType = "Type2"},
            new() {Id = 5, RentType = "Type1"},
            new() {Id = 6, RentType = "Type2"}
        };

        var httpClient = new HttpClient(new TestHttpMessageHandler(expectedEntities, 2));
        var scraper =
            new ApiScraper<ApartmentApiResponse, Apartment>(response => response.Apartments, null, httpClient);

        // Act
        var entities = await scraper.ScrapeAsync(TestApiUrl).ToListAsync();

        // Assert
        CollectionAssert.AreEquivalent(expectedEntities, entities);
    }

    private class TestHttpMessageHandler : HttpMessageHandler
    {
        private readonly List<Apartment> _entities;
        private readonly int _limit;

        public TestHttpMessageHandler(List<Apartment> entities, int limit)
        {
            _entities = entities;
            _limit = limit;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var queryParams = request.RequestUri.ParseQueryString();
            var page = int.Parse(queryParams["page"]);

            var apartments = _entities.Skip((page - 1) * _limit).Take(_limit).ToList();
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent<ApartmentApiResponse>(new ApartmentApiResponse
                {
                    Apartments = apartments,
                    Total = _entities.Count,
                    Page = new Pagination
                    {
                        Limit = _limit,
                        Items = apartments.Count,
                        Current = page,
                        Last = (int) Math.Ceiling((double) _entities.Count / _limit)
                    }
                }, new JsonMediaTypeFormatter())
            };

            return Task.FromResult(response);
        }
    }
}
