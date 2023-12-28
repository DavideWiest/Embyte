namespace Embyte.Modules.Db;

public static class CacheAlg
{
    public static DateTime TimeToRenew(string url, IQueryable<ExtractorEntry> entries)
    {
        if (entries.Count() < 5)
        {
            return DateTime.MinValue;
        }

        return DateTime.Now;    
    }
}
