using ScottPlot;

namespace Analyzer.DataAnalyzer;

internal static class ChartGenerator
{
    public static void CreatePieChart(string[] labels, double[] sizes, string name)
    {
        var plt = new Plot(720, 480);
        plt.Title(name);
        var pie = plt.AddPie(sizes);
        pie.SliceLabels = labels;
        pie.ShowLabels = true;
        plt.SaveFig(name + ".png");
    }

    public static void CreateBarChart(string[] labels, double[] sizes, string name)
    {
        var plt = new Plot(720, 480);
        plt.Title(name);
        plt.AddBar(sizes);
        plt.XTicks(labels);
        plt.SaveFig(name + ".png");
    }

    public static void CreateScatterChart(double[] xValues, double[] yValues, string chartTitle, string xAxisTitle,
        string yAxisTitle)
    {
        var plt = new Plot(720, 480);
        plt.Title(chartTitle);
        plt.XLabel(xAxisTitle);
        plt.YLabel(yAxisTitle);
        plt.Grid(true);
        plt.AddScatter(xValues, yValues, markerShape: MarkerShape.filledCircle, lineWidth: 0, markerSize: 6);
        plt.SaveFig(chartTitle + ".png");
    }
}
