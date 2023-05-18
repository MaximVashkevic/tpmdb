namespace Analyzer.Contract;

public interface IDataReader<T>
{
    IAsyncEnumerable<T> GetRecordsAsync();
}
