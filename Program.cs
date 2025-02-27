using ScottPlot;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;




class Program
{
    static void Main()
    {
        var dataname = "cHouse";
        LinearRegression linearRegression = new LinearRegression();
        linearRegression.RunLinearRegression($"Datasets/{dataname}.csv");

        ComposedRegression composedRegression = new ComposedRegression();
        composedRegression.RunComposedRegression($"Datasets/{dataname}.csv");

    }
}
