using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace Analyzer.Scraper;

public class ApiScraper<TApiResponse, TEntity>
    where TApiResponse : ApiResponse<TEntity>
    where TEntity : class
{
    private readonly Func<TApiResponse, IEnumerable<TEntity>> _getEntities;
    private readonly HttpClient _httpClient;
    private readonly IEnumerable<MediaTypeFormatter> _mediaTypeFormatters;

    public ApiScraper(Func<TApiResponse, IEnumerable<TEntity>> getEntities,
        IEnumerable<MediaTypeFormatter> mediaTypeFormatters, HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:109.0) Gecko/20100101 Firefox/113.0");
        _getEntities = getEntities;
        _mediaTypeFormatters = mediaTypeFormatters;
    }

    public async IAsyncEnumerable<TEntity> ScrapeAsync(string apiUrl)
    {
        var currentPage = 1;
        int lastPage;

        do
        {
            var url = $"{apiUrl}&page={currentPage}";
            using (var response = await _httpClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsAsync<TApiResponse>(_mediaTypeFormatters ??
                        Enumerable.Empty<MediaTypeFormatter>());

                    foreach (var entity in _getEntities(data)) yield return entity;

                    lastPage = data.Page.Last;
                    currentPage++;

                    await Task.Delay(500);
                }
                else
                {
                    break;
                }
            }
        } while (currentPage <= lastPage);
    }
}
