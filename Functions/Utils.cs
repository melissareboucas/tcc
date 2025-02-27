using ScottPlot;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;



public class Utils
{
    public List<string> LoadHeaders(string path)
    {
        using (var reader = new StreamReader(path))
        using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
        {
            var headers = new List<string>();

            // Lê o cabeçalho
            csv.Read();
            csv.ReadHeader();
            headers.AddRange(csv.HeaderRecord); // Salva os nomes das colunas

            return headers;
        }
    }
    public (List<string> headers, List<double> xValues, List<double> yValues) LoadData(string path)
    {
        using (var reader = new StreamReader(path))
        using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
        {
            var headers = new List<string>();
            var xValues = new List<double>();
            var yValues = new List<double>();

            // Lê o cabeçalho
            csv.Read();
            csv.ReadHeader();
            headers.AddRange(csv.HeaderRecord); // Salva os nomes das colunas

            // Lê os dados
            while (csv.Read())
            {
                xValues.Add(csv.GetField<double>(0));
                yValues.Add(csv.GetField<double>(1));
            }

            return (headers, xValues, yValues);
        }
    }

    public (List<double> trainX, List<double> trainY, List<double> testX, List<double> testY) LoadAndSplitData(string path)
    {
        using (var reader = new StreamReader(path))
        using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
        {
            var xValues = new List<double>();
            var yValues = new List<double>();

            // Lê o cabeçalho e ignora
            csv.Read();
            csv.ReadHeader();

            // Lê os dados já ordenados por Y (garantido pelo CSV)
            while (csv.Read())
            {
                xValues.Add(csv.GetField<double>(0));
                yValues.Add(csv.GetField<double>(1));
            }

            // Gera índices aleatórios sem mudar a ordem do Y
            int total = xValues.Count;
            int trainSize = (int)(total * 0.8);

            var indices = Enumerable.Range(0, total).OrderBy(_ => Guid.NewGuid()).ToList(); // Embaralha índices

            var trainIndices = indices.Take(trainSize).ToList();
            var testIndices = indices.Skip(trainSize).ToList();

            // Seleciona os valores correspondentes aos índices aleatórios
            var trainX = trainIndices.Select(i => xValues[i]).ToList();
            var trainY = trainIndices.Select(i => yValues[i]).ToList();
            var testX = testIndices.Select(i => xValues[i]).ToList();
            var testY = testIndices.Select(i => yValues[i]).ToList();

            return (trainX, trainY, testX, testY);
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
