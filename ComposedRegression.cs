using ScottPlot;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;



public class ComposedRegression
{
    public void RunComposedRegression()
    {
        var utils = new Utils();
        // Carrega os dados do arquivo CSV
        (List<double> xTrain, List<double> yTrain) = utils.LoadData("salaryDataTrain.csv");

        List<double> firstX = new List<double>();
        firstX.Add(xTrain[0]);
        firstX.Add(xTrain[1]);

        List<double> firstY = new List<double>();
        firstY.Add(yTrain[0]);
        firstY.Add(yTrain[1]);

        // Calcula os coeficientes da regressão linear
        (double beta0, double beta1) = utils.CalculateCoeficients(firstX, firstY);

        // Calcula os valores para a linha de regressão
        double[] lineXs = [firstX.Min(), 10];
        double[] lineYs = [beta1 * lineXs[0] + beta0, beta1 * lineXs[1] + beta0];

        List<double> secondX = new List<double>();
        secondX.Add(xTrain[1]);
        secondX.Add(xTrain[2]);

        List<double> secondY = new List<double>();
        secondY.Add(yTrain[1]);
        secondY.Add(yTrain[2]);

        // Calcula os coeficientes da regressão linear
        (double beta02, double beta12) = utils.CalculateCoeficients(secondX, secondY);

        // Calcula os valores para a linha de regressão
        double[] lineXs2 = [secondX.Min(), 10];
        double[] lineYs2 = [beta12 * lineXs2[0] + beta02, beta12 * lineXs2[1] + beta02];

        List<double> thirdX = new List<double>();
        thirdX.Add(xTrain[2]);
        thirdX.Add(xTrain[3]);

        List<double> thirdY = new List<double>();
        thirdY.Add(yTrain[2]);
        thirdY.Add(yTrain[3]);

        // Calcula os coeficientes da regressão linear
        (double beta03, double beta13) = utils.CalculateCoeficients(thirdX, thirdY);

        // Calcula os valores para a linha de regressão
        double[] lineXs3 = [thirdX.Min(), 10];
        double[] lineYs3 = [beta13 * lineXs3[0] + beta03, beta13 * lineXs3[1] + beta03];

        // Calcula os valores de xTest, yTest e yPred
        (List<double> xTest, List<double> yTest) = utils.LoadData("salaryDataTest.csv");
        List<double> yPred = new List<double>();
        var y1 = ((beta1 * lineXs[1] + beta0) + (beta12 * lineXs2[1] + beta02))/2;
        yPred.Add((y1 + (beta13 * lineXs3[1] + beta03))/2);

        // Calcula os erros
        (double mae, double mse, double rmse) = utils.CalculateError(yTest, yPred);


        string errosTexto = $"MAE: {mae:F2}\nMSE: {mse:F2}\nRMSE: {rmse:F2}";

        // Cria os gráficos
        var trainData = new ScottPlot.Plot();
        trainData.Add.ScatterPoints(xTrain, yTrain, color: Colors.Blue);
        var line1 = trainData.Add.Line(lineXs[0], lineYs[0], lineXs[1], lineYs[1]);
        line1.Color = Colors.Red;

        var line2 = trainData.Add.Line(lineXs2[0], lineYs2[0], lineXs2[1], lineYs2[1]);
        line2.Color = Colors.Green;

        var line3 = trainData.Add.Line(lineXs3[0], lineYs3[0], lineXs3[1], lineYs3[1]);
        line3.Color = Colors.Purple;

        trainData.Add.Annotation(errosTexto);


        trainData.Add.ScatterPoints(xTest, yPred, color: Colors.Purple);
        var verticalLine = trainData.Add.Line(10, yPred[0], 10, yTest[0]);
        trainData.Add.ScatterPoints(xTest, yTest, color: Colors.Purple);
        verticalLine.Color = Colors.Purple;

        trainData.Title("Regressão Linear - Composição");
        trainData.XLabel("ano");
        trainData.YLabel("salario");

        trainData.SavePng("grafico4.png", 600, 400);
    }
}
