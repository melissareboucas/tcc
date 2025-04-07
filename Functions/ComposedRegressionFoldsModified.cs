using ScottPlot;

public class ComposedRegressionFoldsModified
{
    public void RunComposedRegressionFoldsModified(string pathCsv)
    {
        var utils = new Utils();
        List<string> headers = utils.LoadHeaders(pathCsv);

        int k = 5;
        (List<double> xTrain, List<double> yTrain, List<double> xTest, List<double> yTest) = utils.LoadAndSplitData3(pathCsv, k);
        List<double> yPred = new List<double>();
        List<List<double>> yPredTempList = new List<List<double>>();

        // Cria o gráfico
        var plot = new ScottPlot.Plot();
        plot.Title("Regressão Linear - Composição com método Partição");
        plot.XLabel(headers[0]);
        plot.YLabel(headers[1]);

        int foldSize = xTrain.Count / k;

        for (int foldIndex = 0; foldIndex < k; foldIndex++)
        {
            int start = foldIndex * foldSize;
            int end = Math.Min(start + foldSize, xTrain.Count - 1);

            var yPredTemp = new List<double>(new double[xTest.Count]); // Preenche com zeros
            bool firstModel = true;

            for (int i = start; i < end; i++)
            {
                if (i + 1 >= xTrain.Count)
                    break;

                var xList = new List<double> { xTrain[i], xTrain[i + 1] };
                var yList = new List<double> { yTrain[i], yTrain[i + 1] };

                (double beta0, double beta1) = utils.CalculateCoefficients(xList, yList);

                for (int j = 0; j < xTest.Count; j++)
                {
                    double prediction = beta1 * xTest[j] + beta0;
                    if (firstModel)
                    {
                        yPredTemp[j] = prediction;
                    }
                    else
                    {
                        yPredTemp[j] = (yPredTemp[j] + prediction) / 2.0;
                    }
                }

                firstModel = false;
            }

            yPredTempList.Add(yPredTemp);
        }

        // Média das predições por ponto
        for (int i = 0; i < yTest.Count; i++)
        {
            double media = yPredTempList.Average(list => list[i]);
            yPred.Add(media);
        }

        // Adiciona valores de treino, teste e predição no gráfico
        plot.Add.ScatterPoints(xTrain, yTrain, color: Colors.Blue);
        plot.Add.ScatterPoints(xTest, yTest, color: Colors.Purple);
        plot.Add.ScatterPoints(xTest, yPred, color: Colors.Pink);

        // Calcula os erros
        (double mae, double mse, double rmse) = utils.CalculateError(yTest, yPred);

        // Adiciona valores de erro no gráfico
        string errosTexto = $"MAE: {mae:F2}\nMSE: {mse / 1000:F2}k\nRMSE: {rmse:F2}";
        plot.Add.Annotation(errosTexto);

        // Salva o gráfico
        plot.SavePng("ComposedRegressionFoldsModified.png", 600, 400);
    }
}
