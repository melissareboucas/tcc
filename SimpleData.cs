using ScottPlot;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;



public class SimpleData
{
    public void RunSimpleData()
    {
        var utils = new Utils();
        // Carrega os dados do arquivo CSV
        (List<double> xTrain, List<double> yTrain) = utils.LoadData("salaryDataTrain.csv");

        // Calcula os coeficientes da regressão linear
        (double beta0, double beta1) = utils.CalculateCoeficients(xTrain, yTrain);

        // Calcula os valores para a linha de regressão
        double[] lineXs = [xTrain.Min(), 10];
        double[] lineYs = [beta1 * lineXs[0] + beta0, beta1 * lineXs[1] + beta0];

        // Calcula os valores de xTest, yTest e yPred
        (List<double> xTest, List<double> yTest) = utils.LoadData("salaryDataTest.csv");
        List<double> yPred = xTest.Select(x => beta1 * x + beta0).ToList();

        // Calcula os erros
        (double mae, double mse, double rmse) = utils.CalculateError(yTest, yPred);


        string errosTexto = $"MAE: {mae:F2}\nMSE: {mse:F2}\nRMSE: {rmse:F2}";

        // Cria os gráficos
        var trainData = new ScottPlot.Plot();
        trainData.Add.ScatterPoints(xTrain, yTrain, color: Colors.Blue);
        var line = trainData.Add.Line(lineXs[0], lineYs[0], lineXs[1], lineYs[1]);
        line.Color = Colors.Red;

        trainData.Add.Annotation(errosTexto);


        trainData.Add.ScatterPoints(xTest, yPred, color: Colors.Purple);
        var verticalLine = trainData.Add.Line(10, yPred[0], 10, yTest[0]);
        trainData.Add.ScatterPoints(xTest, yTest, color: Colors.Purple);
        verticalLine.Color = Colors.Purple;

        trainData.Title("Regressão Linear - Dados Simples");
        trainData.XLabel("ano");
        trainData.YLabel("salario");

        trainData.SavePng("grafico3.png", 600, 400);
    }
}
