using System.Globalization;
using Analyzer.Contract;
using Analyzer.Models;

namespace Analyzer;

internal class CsvWriter : IDataWriter<Apartment>
{
    private readonly string _fileName;

    public CsvWriter(string fileName)
    {
        _fileName = fileName;
    }

    public async Task WriteRecordsAsync(IAsyncEnumerable<Apartment> records)
    {
        using (var writer = new StreamWriter(_fileName))
        using (var csv = new CsvHelper.CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteHeader<Apartment>();
            await foreach (var record in records)
            {
                await csv.NextRecordAsync();
                csv.WriteRecord(record);
            }
        }
    }
}
