namespace Analyzer.Scraper;

public abstract class ApiResponse<T>
{
    public int Total { get; set; }
    public Pagination Page { get; set; }
}
