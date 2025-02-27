using ScottPlot;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;



public class SimpleData
{
    public void RunSimpleData(string trainCsv, string testCsv)
    {
        var utils = new Utils();
        // Carrega os dados dos arquivos CSV
        (List<string> headersTrain, List<double> xTrain, List<double> yTrain) = utils.LoadData(trainCsv);
        (List<string> headersTest, List<double> xTest, List<double> yTest) = utils.LoadData(testCsv);

        // Calcula os coeficientes da regressão linear
        (double beta0, double beta1) = utils.CalculateCoeficients(xTrain, yTrain);

        // Calcula os valores para a linha de regressão
        double[] lineXs = [xTrain.Min(), xTest.Max()];
        double[] lineYs = [beta1 * lineXs[0] + beta0, beta1 * lineXs[1] + beta0];

        // Calcula os valores de xTest, yTest e yPred
        List<double> yPred = xTest.Select(x => beta1 * x + beta0).ToList();

        // Calcula os erros
        (double mae, double mse, double rmse) = utils.CalculateError(yTest, yPred);

        string errosTexto = $"MAE: {mae:F2}\nMSE: {mse/1000:F2}k\nRMSE: {rmse:F2}";

        // Cria os gráficos
        var trainData = new ScottPlot.Plot();
        trainData.Add.ScatterPoints(xTrain, yTrain, color: Colors.Blue);
        var line = trainData.Add.Line(lineXs[0], lineYs[0], lineXs[1], lineYs[1]);
        line.Color = Colors.Red;

        trainData.Add.Annotation(errosTexto);

        trainData.Add.ScatterPoints(xTest, yTest, color: Colors.Purple);
        //trainData.Add.ScatterPoints(xTest, yPred, color: Colors.Pink);

        trainData.Title("Regressão Linear - Dados Simples");
        trainData.XLabel(headersTrain[0]);
        trainData.YLabel(headersTrain[1]);

        trainData.SavePng("LinearRegression.png", 600, 400);
    }
}
