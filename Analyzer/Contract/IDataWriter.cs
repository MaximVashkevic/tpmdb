namespace Analyzer.Contract;

public interface IDataWriter<T>
{
    Task WriteRecordsAsync(IAsyncEnumerable<T> records);
}
