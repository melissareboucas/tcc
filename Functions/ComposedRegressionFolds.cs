using ScottPlot;



public class ComposedRegressionFolds
{
    public void RunComposedRegressionFolds(string pathCsv)
    {
        var utils = new Utils();
        // Carrega os dados dos arquivos CSV
        List<string> headers = utils.LoadHeaders(pathCsv);

        int k = 10;
        (List<double> xTrain, List<double> yTrain, List<double> xTest, List<double> yTest) = utils.LoadAndSplitData2(pathCsv, k);
        List<double> yPred = new List<double>();

        List<List<double>> yPredTempList = new List<List<double>>();
        List<double> yPredTemp = new List<double>();

        // Cria o gráfico
        var plot = new ScottPlot.Plot();
        plot.Title("Regressão Linear - Composição");
        plot.XLabel(headers[0]);
        plot.YLabel(headers[1]);

        var foldSize = xTrain.Count / k;
        var j = foldSize;
        var max = foldSize;
        for (int index = 0; index < j; index += foldSize)
        {
            max = index + foldSize;
            for (int i = index; i < max; i++)
            {
                // Monta xList e yList com os valores dos 2 pontos analisados
                var xList = new List<double>();
                var yList = new List<double>();

                xList.Add(xTrain[i]);
                xList.Add(xTrain[i + 1]);

                yList.Add(yTrain[i]);
                yList.Add(yTrain[i + 1]);

                // Calcula os coeficientes da reta
                (double beta0, double beta1) = utils.CalculateCoeficients(xList, yList);

                // Calcula/atualiza os valores de yTemp
                if (i == 0)
                {
                    foreach (var x in xTest)
                    {
                        yPredTemp.Add(beta1 * x + beta0);
                    }
                }
                else
                {
                    var aux = 0;
                    foreach (var x in xTest)
                    {
                        var med = (yPredTemp[aux] + (beta1 * x + beta0)) / 2;
                        yPredTemp[aux] = med;
                        aux++;
                    }
                }

                if (max >= xTrain.Count)
                {
                    max = xTrain.Count-1;
                }
            }

            yPredTempList.Add(yPredTemp);
            if (index < xTrain.Count - foldSize)
            {
                j += foldSize;
            }
        }

        int count = yPredTempList[0].Count;
        for (int i = 0; i < count; i++)
        {
            yPred.Add(yPredTempList.Average(list => list[i]));
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

        //Salva o gráfico
        plot.SavePng("ComposedRegressionFolds.png", 600, 400);

    }
}
