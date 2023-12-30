namespace EmbyteTests;
using Embyte.Modules.Product;

public class Tests
{
    ExponentialFitter ef;

    [SetUp]
    public void Setup()
    {
        ef = new ExponentialFitter(100, 0.1);
    }

    [Test]
    public void Test1()
    {
        bool[] booleanData = { true, true, false, true, false, true, false, false };
        double[] time = Enumerable.Range(1, 100).Select(x => (double)x / 10).Take(booleanData.Length).ToArray();

        double a = ef.GetCoefficientForCacheData(booleanData, time);

        Console.WriteLine(a);

        Assert.Pass();
    }
}