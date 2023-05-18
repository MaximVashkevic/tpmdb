using System.Globalization;
using Analyzer.Contract;
using CsvHelper;

namespace Analyzer;

public class CsvReader<T> : IDataReader<T>
{
    private readonly CsvReader _csv;

    public CsvReader(string path)
    {
        var reader = new StreamReader(new FileStream(path, FileMode.Open));
        _csv = new CsvReader(reader, CultureInfo.InvariantCulture);
    }

    public IAsyncEnumerable<T> GetRecordsAsync()
    {
        return _csv.GetRecordsAsync<T>();
    }
}
