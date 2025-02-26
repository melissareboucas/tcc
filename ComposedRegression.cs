using ScottPlot;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Drawing;



public class ComposedRegression
{
    public void RunComposedRegression(string trainCsv, string testCsv)
    {
        var utils = new Utils();
        // Carrega os dados dos arquivos CSV
        (List<string> headersTrain, List<double> xTrain, List<double> yTrain) = utils.LoadData(trainCsv);
        (List<string> headersTest, List<double> xTest, List<double> yTest) = utils.LoadData(testCsv);
        List<double> yPred = new List<double>();

        // Cria o gráfico
        var trainData = new ScottPlot.Plot();
        trainData.Title("Regressão Linear - Composição");
        trainData.XLabel(headersTrain[0]);
        trainData.YLabel(headersTrain[1]);

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
            (double beta0, double beta1) = utils.CalculateCoeficients(xList, yList);

            // Calcula os valores para montar a reta
            double[] lineXs = [xList[0], xTest.Max()];
            double[] lineYs = [beta1 * lineXs[0] + beta0, beta1 * lineXs[1] + beta0];

            // Cria a reta no gráfico
           // trainData.Add.Line(lineXs[0], lineYs[0], lineXs[1], lineYs[1]);

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
                    var med = (yPred[index] + (beta1*x + beta0))/2;
                    yPred[index] = med;
                    index++;
                }
            }

        }

        // Adiciona valores de treino, teste e predição no gráfico
        trainData.Add.ScatterPoints(xTrain, yTrain, color: Colors.Blue);
       // trainData.Add.ScatterPoints(xTest, yTest, color: Colors.Purple);
       // trainData.Add.ScatterPoints(xTest, yPred, color: Colors.Pink);

        // Calcula os erros
        (double mae, double mse, double rmse) = utils.CalculateError(yTest, yPred);

        // Adiciona valores de erro no gráfico
        string errosTexto = $"MAE: {mae:F2}\nMSE: {mse/1000:F2}k\nRMSE: {rmse:F2}";
        trainData.Add.Annotation(errosTexto);

        //Salva o gráfico
        trainData.SavePng("ComposedRegression.png", 600, 400);
    }
}
