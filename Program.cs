using ScottPlot;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;




class Program
{
    static void Main()
    {
        LinearRegression linearRegression = new LinearRegression();
        //linearRegression.RunLinearRegression("Datasets/salaryDataTrain.csv","Datasets/salaryDataTest.csv");

        SimpleData simpleData = new SimpleData();
        simpleData.RunSimpleData("Datasets/CaliforniaOrderedNoDuplicatesTrain.csv","Datasets/CaliforniaOrderedNoDuplicatesTest.csv");

        ComposedRegression composedRegression = new ComposedRegression();
        composedRegression.RunComposedRegression("Datasets/CaliforniaOrderedNoDuplicatesTrain.csv","Datasets/CaliforniaOrderedNoDuplicatesTest.csv");
    }
}
