using ScottPlot;



public class LinearRegression
{
    public void RunLinearRegression(string pathCsv)
    {
        var utils = new Utils();
        // Carrega os dados dos arquivos CSV
        List<string> headers = utils.LoadHeaders(pathCsv);
        (List<double> xTrain, List<double> yTrain, List<double> xTest, List<double> yTest) = utils.LoadAndSplitData(pathCsv);
        List<double> yPred = new List<double>();

        // Cria o gráfico
        var plot = new ScottPlot.Plot();
        plot.Title("Regressão Linear - Método 80/20");
        plot.XLabel(headers[0]);
        plot.YLabel(headers[1]);


        // Calcula os coeficientes da regressão linear
        (double beta0, double beta1) = utils.CalculateCoefficients(xTrain, yTrain);

        // Calcula os valores para a linha de regressão
        double[] lineXs = [xTrain.Min(), xTest.Max()];
        double[] lineYs = [beta1 * lineXs[0] + beta0, beta1 * lineXs[1] + beta0];

        // Calcula os valores de yPred
        yPred = xTest.Select(x => beta1 * x + beta0).ToList();

        // Adiciona valores de treino, teste e predição no gráfico
        plot.Add.ScatterPoints(xTest, yTest, color: Colors.Purple);
        plot.Add.ScatterPoints(xTrain, yTrain, color: Colors.Blue);
        plot.Add.ScatterPoints(xTest, yPred, color: Colors.Pink);

        // Calcula os erros
        (double mae, double mse, double rmse) = utils.CalculateError(yTest, yPred);

        // Adiciona valores de erro no gráfico
        string errosTexto = $"MAE: {mae:F2}\nMSE: {mse:F2}\nRMSE: {rmse:F2}";
        plot.Add.Annotation(errosTexto);

        //Salva o gráfico
        plot.SavePng("LinearRegression.png", 600, 400);
    }
}
