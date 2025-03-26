using ScottPlot;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Drawing;



public class ComposedRegressionAVG
{
    public void RunComposedRegressionAVG(string pathCsv)
    {
        var utils = new Utils();
        // Carrega os dados dos arquivos CSV
        List<string> headers = utils.LoadHeaders(pathCsv);
        (List<double> xTrain, List<double> yTrain, List<double> xTest, List<double> yTest) = utils.LoadAndSplitData(pathCsv);

        // Cria variáveis de pred
        List<double> yPredTemp = new List<double>();
        List<List<double>> listYPredTemp = new List<List<double>>();
        List<double> yPred = new List<double>();

        // Cria o gráfico
        var plot = new ScottPlot.Plot();
        plot.Title("Regressão Linear - Composição");
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
            double[] lineXs = [xList[0], xTrain.Max()];
            double[] lineYs = [beta1 * lineXs[0] + beta0, beta1 * lineXs[1] + beta0];

            // Cria a reta no gráfico
            //plot.Add.Line(lineXs[0], lineYs[0], lineXs[1], lineYs[1]);


            // Calcula os valores de ypred
            foreach (var x in xTest)
            {
                yPredTemp.Add(beta1 * x + beta0);
            }

            //Adiciona ypred nas lista de ypred
            listYPredTemp.Add(yPredTemp);
        }

        foreach (var ypredTemp in listYPredTemp)
        {
            double sum = ypredTemp.Sum();
            double avg = sum / ypredTemp.Count;
            yPred.Add(avg);
        }

        // Adiciona valores de treino, teste e predição no gráfico
        plot.Add.ScatterPoints(xTrain, yTrain, color: Colors.Blue);
        plot.Add.ScatterPoints(xTest, yTest, color: Colors.Purple);
        plot.Add.ScatterPoints(xTest, yPredTemp, color: Colors.Pink);

        // Calcula os erros
        (double mae, double mse, double rmse) = utils.CalculateError(yTest, yPredTemp);

        // Adiciona valores de erro no gráfico
        string errosTexto = $"MAE: {mae:F2}\nMSE: {mse / 1000:F2}k\nRMSE: {rmse:F2}";
        plot.Add.Annotation(errosTexto);

        //Salva o gráfico
        plot.SavePng("ComposedRegressionAVG.png", 600, 400);

    }
}
