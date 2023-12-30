namespace Embyte.Modules.Product;

public class ExponentialFitter
{
    public double lr;
    public int iterations; 

    public ExponentialFitter(int iterations, double lr=0.1)
    {
        this.lr = lr;
        this.iterations = iterations;
    }

    public double GetCoefficientForCacheData(bool[] data, double[] time)
    {
        if (data.Length != time.Length)
            throw new ArgumentException("Input arrays must be of the same length.");

        double[] timeDeltas = time.Skip(1).Select((t, i) => t - time[i]).ToArray();
        double[] actual = data.Select(b => b ? 1.0 : 0.0).ToArray();

        double a = -1;
        for (int i = 0; i<iterations; i++)
        {
            double[] pred = timeDeltas.Select(t => Math.Exp(a * t)).ToArray();
            double loss = GetLoss(pred, actual);
            double gradient = loss * timeDeltas.Select((t, i) => t * pred[i]).Sum();

            a -= lr * gradient;
        }

        return a;
    }

    public double GetLoss(double[] pred, double[] actual)
    {
        return Math.Sqrt(pred.Select((x, i) => Math.Pow(x - actual[i], 2.0)).Sum());
    }
}
