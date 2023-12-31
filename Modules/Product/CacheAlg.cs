namespace Embyte.Modules.Product;

using Accord.Math;
using Embyte.Data.Storage;
using Embyte.Modules.Logging;
using Meta.Numerics.Functions;

public static class CacheAlg
{
    public static DateTime TimeToRenew(string url, IQueryable<RequestEntry> entriesGeneral)
    {
        var entriesSpecific = entriesGeneral.Where(e => e.Url == url);

        if (TooFewDataPoints(entriesSpecific) || TooOldDataPoints(entriesSpecific))
            return DateTime.MinValue;

        if (TooRecent(entriesSpecific))
            return DateTime.MaxValue;

        double ratioImportanceHitMiss = 0.1;
        double lowerLimitHours = 0.1;

        var (timeStart, entriesForFitting) = ChooseEntries(entriesGeneral, entriesSpecific, lowerLimitHours);
        double coeff = GetPolyfittedSecondCoefficient(entriesForFitting, timeStart, lowerLimitHours);
        var t = timeStart.Add(TimeSpan.FromHours(CalculateTimeToRenewFromPolyfittedArguments(ratioImportanceHitMiss, coeff)));

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

    public static bool TooOldDataPoints(IQueryable<RequestEntry> entriesSpecificToUrl)
    {
        var mostRecentEntry = entriesSpecificToUrl
            .OrderByInDb("Time", true)
            .FirstOrDefault();

        var cond = mostRecentEntry != null && mostRecentEntry.Time < DateTime.Now.AddMonths(-EmbyteStorage.upperLimitCacheAgeMonths);

        Log.Debug("TooOldDataPoints: {cond}", cond);
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

    public static double GetPolyfittedSecondCoefficient(IQueryable<RequestEntry> entries, DateTime timeStart, double lowerLimitHours)
    {
        double[] time = entries.Select(e => e.Time.Subtract(timeStart).TotalHours).ToArray();
        // The exponential function will be fitted to the probability of data not having changed over time, because
        // it is the one declining over time. Hence the negation
        bool[] booleanData = entries.Select(e => !e.DataChanged).ToArray();
        double[] timeDeltas = timeDeltasOfEntries(entries);

        booleanData = booleanData.Skip(1).ToArray();
        booleanData = booleanData.Where((b, i) => timeDeltas[i] > lowerLimitHours).ToArray();
        timeDeltas = timeDeltas.Where(t => t > lowerLimitHours).ToArray();
        double a = new ExponentialFitter(100, 0.5).GetCoefficientForCacheData(booleanData, timeDeltas, out double mse, 0.00001);

        return a;
    }

    public static Tuple<DateTime, IQueryable<RequestEntry>> ChooseEntries(IQueryable<RequestEntry> entriesSpecific, IQueryable<RequestEntry> entriesGeneral, double lowerLimitHours)
    {
        double[] timeDeltasSpecific = timeDeltasOfEntries(entriesSpecific);
        double[] timeDeltasGeneral = timeDeltasOfEntries(entriesGeneral);

        IQueryable<RequestEntry> entries = timeDeltasSpecific.Where(t => t > lowerLimitHours).Count() >= 8 ? entriesSpecific : entriesGeneral;
        DateTime timeStart = entries
            .OrderByInDb("Time", false)
            .First().Time;

        return Tuple.Create(timeStart, entries);
    }

    public static double[] timeDeltasOfEntries(IQueryable<RequestEntry> entries)
    {
        DateTime[] time = entries.Select(e => e.Time).ToArray();
        return time.Skip(1).Select((t, i) => t.Subtract(time[i]).TotalHours).ToArray();
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