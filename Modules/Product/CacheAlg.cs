namespace Embyte.Modules.Product;
using Accord.Statistics.Models.Regression;
using Accord.Statistics.Models.Regression.Fitting;
using Embyte.Modules.Logging;
using Meta.Numerics.Functions;

public static class CacheAlg
{
    public static DateTime TimeToRenew(string url, IQueryable<RequestEntry> entriesGeneral)
    {
        var entriesSpecific = entriesGeneral.Where(e => e.Url == url);

        if (
            TooFewDataPoints(entriesSpecific)
            // DEVELOPMENT
            //|| TooRecent(entriesSpecific)
            || NoRecentChange(entriesSpecific)
        )
            return DateTime.MinValue;

        var entriesForPolyfitting = entriesSpecific.Count() > 10 ? entriesSpecific : entriesGeneral;

        var timeStart = entriesForPolyfitting
            .OrderByInDb("Time", false)
            .First().Time.ToOADate();
        var coeff = GetPolyfittedSecondCoefficient(entriesForPolyfitting, timeStart);
        var t = DateTime.FromOADate(timeStart + CalculateTimeToRenewFromPolyfittedArguments(0.1, coeff));

        Log.Debug("TimeToRenew Coeff a={coeff}", coeff);
        Log.Debug("TimeToRenew t={t}", t.ToString());

        return t;
    }

    public static bool TooFewDataPoints(IQueryable<RequestEntry> entriesSpecificToUrl)
    {
        var cond = entriesSpecificToUrl.Count() < 5;
        Log.Debug("TooFewDataPoints: {cond}", cond);
        return cond;
    }

    public static bool TooRecent(IQueryable<RequestEntry> entriesSpecificToUrl)
    {
        var cond = entriesSpecificToUrl
            .OrderByInDb("Time", true)
            .First().Time > DateTime.Now.AddHours(-12);
        Log.Debug("TooRecent: {cond}", cond);
        return cond;
    }

    public static bool NoRecentChange(IQueryable<RequestEntry> entriesSpecificToUrl)
    {
        var recentEntries = entriesSpecificToUrl
            .Where(e => e.DataChanged)
            .OrderByInDb("Time", true);
        var cond = recentEntries.FirstOrDefault() != null && !(recentEntries.First().Time < DateTime.Now.AddYears(-1));
        Log.Debug("NoRecentChange: {cond}", cond);
        return cond;
    }

    public static double GetPolyfittedSecondCoefficient(IQueryable<RequestEntry> entries, double timeStart)
    {
        double[] time = entries.Select(e => e.Time.ToOADate() - timeStart).ToArray();
        bool[] booleanData = entries.Select(e => e.DataChanged).ToArray();
        
        Log.Debug(string.Join(", ", time.ToList()));
        Log.Debug(string.Join(", ", booleanData));

        double a = new ExponentialFitter(100, 0.1).GetCoefficientForCacheData(booleanData, time);

        return a;
    }

    public static double CalculateTimeToRenewFromPolyfittedArguments(double ratioImportanceHitMiss, double fittedSecondCoefficient)
    {
        if (ratioImportanceHitMiss < 0 || ratioImportanceHitMiss > 1)
            throw new ArgumentException("ratioImportanceHitMiss must be in the range of 0 and 1 (inclusive)");

        var a = fittedSecondCoefficient;
        var c = ratioImportanceHitMiss;

        return -(1/a) * AdvancedMath.LambertW(-(c+1)/c*Math.Exp(-(c+1)/c)) -(c+1)/(a*c);
    }
}