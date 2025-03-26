using ScottPlot;



public class LinearRegressionFolds
{
    public void RunLinearRegressionFolds(string pathCsv)
    {
        var utils = new Utils();

        // Carrega os dados dos arquivos CSV
        List<string> headers = utils.LoadHeaders(pathCsv);

        //define a quantidade de partições
        int k = 10;
        (List<double> xTrain, List<double> yTrain, List<double> xTest, List<double> yTest) = utils.LoadAndSplitData2(pathCsv, k);
        List<double> yPred = new List<double>();

        // Cria o gráfico
        var plot = new ScottPlot.Plot();
        plot.Title("Regressão Linear - Método Partição");
        plot.XLabel(headers[0]);
        plot.YLabel(headers[1]);


        // Calcula os coeficientes da regressão linear
        double totalBeta0 = 0;
        double totalBeta1 = 0;

        var foldSize = xTrain.Count / k;
        var j = foldSize;
        var max = foldSize;
        for (int index = 0; index < j; index += foldSize)
        {
            (double beta0, double beta1) = utils.CalculateCoefficients(xTrain, yTrain);
            totalBeta0 += beta0;
            totalBeta1 += beta1;
            if (index < xTrain.Count - foldSize)
            {
                j += foldSize;
            }
        }

        double avgBeta0 = totalBeta0 / k;
        double avgBeta1 = totalBeta1 / k;

        // Calcula os valores de yPred
        yPred = xTest.Select(x => avgBeta1 * x + avgBeta0).ToList();

        // Adiciona valores de treino, teste e predição no gráfico
        plot.Add.ScatterPoints(xTest, yTest, color: Colors.Purple);
        plot.Add.ScatterPoints(xTrain, yTrain, color: Colors.Blue);
        plot.Add.ScatterPoints(xTest, yPred, color: Colors.Pink);


        // Calcula os erros
        (double mae, double mse, double rmse) = utils.CalculateError(yTest, yPred);

        // Adiciona valores de erro no gráfico
        string errosTexto = $"MAE: {mae:F2}\nMSE: {mse / 1000:F2}k\nRMSE: {rmse:F2}";
        plot.Add.Annotation(errosTexto);

        //Salva o gráfico
        plot.SavePng("LinearRegressionFolds.png", 600, 400);
    }
}
