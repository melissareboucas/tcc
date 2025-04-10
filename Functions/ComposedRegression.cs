using ScottPlot;



public class ComposedRegression
{
    public void RunComposedRegression(string pathCsv)
    {
        var utils = new Utils();
        // Carrega os dados dos arquivos CSV
        List<string> headers = utils.LoadHeaders(pathCsv);
        (List<double> xTrain, List<double> yTrain, List<double> xTest, List<double> yTest) = utils.LoadAndSplitDataTemporal(pathCsv);
        List<double> yPred = new List<double>();

        // Cria o gráfico
        var plot = new ScottPlot.Plot();
        plot.Title("Regressão Linear - Composição com método 80/20");
        plot.XLabel(headers[0]);
        plot.YLabel(headers[1]);

        for (int i = 0; i < xTrain.Count - 1; i++)
        {
            // Monta xList e yList com os valores dos 2 pontos analisados
            var xList = new List<double>();
            var yList = new List<double>();

            xList.Add(xTrain[i]);
            xList.Add(xTrain[i + 1]);

            yList.Add(yTrain[i]);
            yList.Add(yTrain[i + 1]);

            // Calcula os coeficientes da reta
            (double beta0, double beta1) = utils.CalculateCoefficients(xList, yList);

            // Calcula os valores para montar a reta
            double[] lineXs = [xList[0], xTest.Max()];
            double[] lineYs = [beta1 * lineXs[0] + beta0, beta1 * lineXs[1] + beta0];

            // Cria a reta no gráfico
            //plot.Add.Line(lineXs[0], lineYs[0], lineXs[1], lineYs[1]);

            // Calcula/atualiza os valores de ypred
            if (i == 0)
            {
                foreach (var x in xTest)
                {
                    yPred.Add(beta1 * x + beta0);
                }
            }
            else
            {
                var index = 0;
                foreach (var x in xTest)
                {
                    var med = (yPred[index] + (beta1 * x + beta0)) / 2;
                    yPred[index] = med;
                    index++;
                }
            }

        }

        // Adiciona valores de treino, teste e predição no gráfico
        plot.Add.ScatterPoints(xTrain, yTrain, color: Colors.Blue);
        plot.Add.ScatterPoints(xTest, yTest, color: Colors.Purple);
        plot.Add.ScatterPoints(xTest, yPred, color: Colors.Pink);

        // Calcula os erros
        (double mae, double mse, double rmse) = utils.CalculateError(yTest, yPred);

        // Adiciona valores de erro no gráfico
        string errosTexto = $"MAE: {mae:F2}\nMSE: {mse:F2}\nRMSE: {rmse:F2}";
        plot.Add.Annotation(errosTexto);

        //Salva o gráfico
        plot.SavePng("ComposedRegression.png", 600, 400);

    }

    public void RunComposedRegressionWeighted(string pathCsv)
    {
        var utils = new Utils();
        List<string> headers = utils.LoadHeaders(pathCsv);
        (List<double> xTrain, List<double> yTrain, List<double> xTest, List<double> yTest) = utils.LoadAndSplitDataTemporal(pathCsv);

        List<double> yPred = new List<double>(new double[xTest.Count]);
        List<double> weightSum = new List<double>(Enumerable.Repeat(0.0, xTest.Count));

        double alpha = 1.0; // define o "ritmo" da queda do peso com a distância temporal

        var plot = new ScottPlot.Plot();
        plot.Title("Regressão Linear Ponderada por Tempo");
        plot.XLabel(headers[0]);
        plot.YLabel(headers[1]);

        for (int i = 0; i < xTrain.Count - 1; i++)
        {
            var xList = new List<double> { xTrain[i], xTrain[i + 1] };
            var yList = new List<double> { yTrain[i], yTrain[i + 1] };

            (double beta0, double beta1) = utils.CalculateCoefficients(xList, yList);

            // Centro temporal da reta
            double centerX = (xList[0] + xList[1]) / 2.0;

            for (int j = 0; j < xTest.Count; j++)
            {
                double distance = Math.Abs(xTest[j] - centerX);
                double weight = Math.Exp(-alpha * distance);
                double pred = beta1 * xTest[j] + beta0;

                yPred[j] += pred * weight;
                weightSum[j] += weight;
            }
        }

        // Finaliza cálculo da média ponderada
        for (int j = 0; j < yPred.Count; j++)
        {
            yPred[j] = yPred[j] / weightSum[j];
        }

        // Gráficos
        plot.Add.ScatterPoints(xTrain, yTrain, color: Colors.Blue);
        plot.Add.ScatterPoints(xTest, yTest, color: Colors.Purple);
        plot.Add.ScatterPoints(xTest, yPred, color: Colors.Orange);

        (double mae, double mse, double rmse) = utils.CalculateError(yTest, yPred);
        string errosTexto = $"MAE: {mae:F2}\nMSE: {mse:F2}\nRMSE: {rmse:F2}";
        plot.Add.Annotation(errosTexto);

        plot.SavePng("ComposedRegression_Weighted.png", 600, 400);
    }

}
