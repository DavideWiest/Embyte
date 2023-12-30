using Accord.Statistics;

namespace Embyte.Modules.Product;

public class ExponentialFitter
{
    public double lr;
    public int iterations; 

    public ExponentialFitter(int iterations, double lr)
    {
        this.lr = lr;
        this.iterations = iterations;
    }

    public double GetCoefficientForCacheData(bool[] data, double[] timeDeltas, out double mse, double epsilon)
    {
        if (data.Length != timeDeltas.Length)
            throw new ArgumentException($"Input arrays must be of the same length: data.Length={data.Length}, timeDeltas.Length={timeDeltas.Length}");

        double[] actual = data.Select(b => b ? 1.0 : 0.0).ToArray();

        double a = -1;
        mse = 0.0;
        double msePrev = 0.0;

        for (int i = 0; i<iterations; i++)
        {
            double[] pred = timeDeltas.Select(t => Math.Exp(a * t)).ToArray();
            msePrev = mse;
            mse = GetLoss(pred, actual);

            double gradient = pred.Select((p, i) => p - actual[i]).Sum() * 2 / pred.Length;
            a -= lr * mse * gradient;

            if (msePrev - mse < epsilon && i > 1)
                break;
        }

        return a;
    }

    public double GetLoss(double[] pred, double[] actual)
    {
        return pred.Select((x, i) => Math.Pow(x - actual[i], 2.0)).Sum() / pred.Length;
    }
}
