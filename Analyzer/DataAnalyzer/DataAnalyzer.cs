using Analyzer.Contract;
using Analyzer.Models;

namespace Analyzer.DataAnalyzer;

public static class DataAnalyzer
{
    public static async Task AnalyzeAsync(IDataReader<Apartment> reader)
    {
        var records = await reader.GetRecordsAsync().ToListAsync();

        if (!records.Any())
        {
            Console.WriteLine("No records found.");
            return;
        }

        CreateOwnerAgencyAuthorPieChart(records);
        CreatePriceRangeBarChartAsync(records);
        CreateRentTypeCharts(records);
        CreateRentPricesPerTypeCharts(records);
        CreateLocationScatterChart(records);
    }

    private static void CreateOwnerAgencyAuthorPieChart(IEnumerable<Apartment> apartments)
    {
        var ownerCount = apartments.Count(a => a.Contact.Owner);
        var agencyCount = apartments.Count(a => !a.Contact.Owner);

        var labels = new[] {"Собственник", "Агентство"};
        var sizes = new double[] {ownerCount, agencyCount};

        ChartGenerator.CreatePieChart(labels, sizes, "Собственник vs Агентство");
    }

    private static void CreatePriceRangeBarChartAsync(IEnumerable<Apartment> apartments)
    {
        var bucketSize = (int) (apartments.Max(r => r.Price.Amount) / 10);

        var priceBars =
            apartments
                .GroupBy(r => Math.Floor(r.Price.Amount / bucketSize))
                .Select(g => (double) g.Count())
                .ToArray();

        var priceLabels = priceBars
            .Select((_, i) => $"{i * bucketSize} - {(i + 1) * bucketSize}")
            .ToArray();

        ChartGenerator.CreateBarChart(priceLabels, priceBars, "Количество объявлений в ценовых диапазонах");
    }

    private static void CreateRentTypeCharts(IEnumerable<Apartment> apartments)
    {
        var rentTypeData = apartments
            .GroupBy(r => r.RentType)
            .Select(r => (RentType: r.Key, Count: r.Count()))
            .OrderBy(r => r.RentType)
            .ToList();

        var rentTypeLabels = rentTypeData.Select(td => td.RentType).ToArray();
        var rentTypeCounts = rentTypeData.Select(td => (double) td.Count).ToArray();

        ChartGenerator.CreateBarChart(rentTypeLabels, rentTypeCounts, "Количество объявлений по типу аренды");
    }

    private static void CreateQuantity(IEnumerable<Apartment> apartments, string name)
    {
        var bucketSize = (int) (apartments.Max(r => r.Price.Amount) / 10);

        var priceBars = apartments
            .GroupBy(r => Math.Floor(r.Price.Amount / bucketSize))
            .Select(g => (double) g.Count()).ToArray();

        var priceLabels = priceBars.Select((_, i) => $"{i * bucketSize} - {(i + 1) * bucketSize}").ToArray();

        ChartGenerator.CreateBarChart(priceLabels, priceBars, name);
    }

    private static void CreateLocationScatterChart(IEnumerable<Apartment> apartments)
    {
        var xValues = apartments.Select(a => a.Location.Longitude).ToArray();
        var yValues = apartments.Select(a => a.Location.Latitude).ToArray();

        ChartGenerator.CreateScatterChart(xValues, yValues, "Разброс по местоположению", "Долгота", "Широта");
    }

    private static void CreateRentPricesPerTypeCharts(IEnumerable<Apartment> apartments)
    {
        var rentTypeData = apartments
            .GroupBy(r => r.RentType);

        foreach (var rentTypeGroup in rentTypeData)
            CreateQuantity(rentTypeGroup, $"Количество объявлений для {rentTypeGroup.Key} в ценовых диапазонах");
    }
}
