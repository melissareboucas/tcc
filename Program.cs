using ScottPlot;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;




class Program
{
    static void Main()
    {
        LinearRegression linearRegression = new LinearRegression();
        //linearRegression.RunLinearRegression();

        SimpleData simpleData = new SimpleData();
        simpleData.RunSimpleData();

        ComposedRegression composedRegression = new ComposedRegression();
        composedRegression.RunComposedRegression();
    }
}
