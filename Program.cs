using ScottPlot;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;




class Program
{
    static void Main()
    {
        var dataname = "mpg";
        LinearRegression linearRegression = new LinearRegression();
        linearRegression.RunLinearRegression($"Datasets/{dataname}.csv");

        ComposedRegression composedRegression = new ComposedRegression();
        composedRegression.RunComposedRegression($"Datasets/{dataname}.csv");

        ComposedRegressionFolds composedRegressionFolds = new ComposedRegressionFolds();
        composedRegressionFolds.RunComposedRegressionFolds($"Datasets/{dataname}.csv");

        LinearRegressionFolds linearRegressionFolds = new LinearRegressionFolds();
        linearRegressionFolds.RunLinearRegressionFolds($"Datasets/{dataname}.csv");
    }
}
