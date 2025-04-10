using ScottPlot;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;




class Program
{
    static void Main()
    {
        var dataname = "mrt";
        LinearRegression linearRegression = new LinearRegression();
        linearRegression.RunLinearRegression($"Datasets/{dataname}.csv");

        ComposedRegression composedRegression = new ComposedRegression();
        composedRegression.RunComposedRegression($"Datasets/{dataname}.csv");
        composedRegression.RunComposedRegressionWeighted($"Datasets/{dataname}.csv");

        ComposedRegressionFolds composedRegressionFolds = new ComposedRegressionFolds();
        //composedRegressionFolds.RunComposedRegressionFolds($"Datasets/{dataname}.csv");

        ComposedRegressionFoldsModified composedRegressionFoldsModified = new ComposedRegressionFoldsModified();
        //composedRegressionFoldsModified.RunComposedRegressionFoldsModified($"Datasets/{dataname}.csv");

        LinearRegressionFolds linearRegressionFolds = new LinearRegressionFolds();
        //linearRegressionFolds.RunLinearRegressionFolds($"Datasets/{dataname}.csv");

        LinearRegressionFoldsModified linearRegressionFoldsModified = new LinearRegressionFoldsModified();
        //linearRegressionFoldsModified.RunLinearRegressionFoldsModified($"Datasets/{dataname}.csv");
    }
}
