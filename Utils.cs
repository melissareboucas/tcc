using ScottPlot;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;



public class Utils
{
    public (List<double> xValues, List<double> yValues) LoadData(string path)
    {
        {
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                var xValues = new List<double>();
                var yValues = new List<double>();
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    xValues.Add(csv.GetField<double>(0));
                    yValues.Add(csv.GetField<double>(1));
                }
                return (xValues, yValues);
            }
        }
    }

    public (double mae, double mse, double rmse) CalculateError(List<double> yTest, List<double> yPred)
    {
        double absoluteErrorSum = 0;
        double squareErrorSum = 0;
        for (int i = 0; i < yTest.Count; i++)
        {
            double error = yTest[i] - yPred[i];
            absoluteErrorSum += Math.Abs(error);
            squareErrorSum += Math.Pow(error, 2);
        }

        // Calcula métricas de erro
        double mae = absoluteErrorSum / yTest.Count;
        double mse = squareErrorSum / yTest.Count;
        double rmse = Math.Sqrt(mse);
        return (mae, mse, rmse);
    }

    public (double beta0, double beta1) CalculateCoeficients(List<double> x, List<double> y)
    {
        if (x.Count != y.Count || x.Count == 0)
            throw new ArgumentException("As listas de valores x e y devem ter o mesmo tamanho e não podem estar vazias.");

        int n = x.Count;
        double somaX = x.Sum();
        double somaY = y.Sum();
        double somaXY = x.Zip(y, (xi, yi) => xi * yi).Sum();
        double somaX2 = x.Sum(xi => xi * xi);

        double beta1 = (n * somaXY - somaX * somaY) / (n * somaX2 - somaX * somaX);
        double beta0 = (somaY - beta1 * somaX) / n;

        return (beta0, beta1);
    }
}
