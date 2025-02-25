using ScottPlot;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;



public class LinearRegression
{
    public void RunLinearRegression()
    {
        var utils = new Utils();
        // Carrega os dados do arquivo CSV
        (List<double> xTrain, List<double> yTrain) = utils.LoadData("CaliforniaTrain.csv");

        // Calcula os coeficientes da regressão linear
        (double beta0, double beta1) = utils.CalculateCoeficients(xTrain, yTrain);

        // Calcula os valores para a linha de regressão
        double[] lineXs = [xTrain.Min(), (500000-beta0)/beta1];
        double[] lineYs = [beta1 * lineXs[0] + beta0, 500000];

        // Calcula os valores de xTest, yTest e yPred
        (List<double> xTest, List<double> yTest) = utils.LoadData("CaliforniaTest.csv");
        List<double> yPred = xTest.Select(x => beta1 * x + beta0).ToList();

        // Calcula os erros
        (double mae, double mse, double rmse) = utils.CalculateError(yTest, yPred);

        // Cria os gráficos
        var trainData = new ScottPlot.Plot();
        trainData.Add.ScatterPoints(xTrain, yTrain, color: Colors.Blue);
        var line = trainData.Add.Line(lineXs[0], lineYs[0], lineXs[1], lineYs[1]);
        line.Color = Colors.Red;

        trainData.Title("Regressão Linear - Dados de Treino");
        trainData.XLabel("medIncome");
        trainData.YLabel("medHouseValue");

        trainData.SavePng("grafico.png", 600, 400);

        var predictionData = new ScottPlot.Plot();
        predictionData.Add.ScatterPoints(xTest, yPred, color: Colors.Green);
        predictionData.Add.ScatterPoints(xTest, yTest, color: Colors.Blue);

        predictionData.Title("Regressão Linear - Dados previstos Vs Dados reais");
        predictionData.XLabel("medIncome");
        predictionData.YLabel("medHouseValue");

        string errosTexto = $"MAE: {mae/1000:F2}K\nMSE: {mse/1000:F2}K\nRMSE: {rmse/1000:F2}K";
        predictionData.Add.Annotation(errosTexto);

        predictionData.SavePng("grafico2.png", 600, 400);

        Console.WriteLine($"MAE: {mae}");
        Console.WriteLine($"MSE: {mse}");
        Console.WriteLine($"RMSE: {rmse}");
    }

}
