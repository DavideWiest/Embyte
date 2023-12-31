using Embyte.Data.Product;
using Embyte.Modules.Db;
using Embyte.Data.Storage;

namespace Embyte.Modules.Product;

public class WebsiteInfoGetter
{
    EmbyteDbContext DbCtx;

    public WebsiteInfoGetter(EmbyteDbContext dbctx)
    {
        DbCtx = dbctx;
    }


    /// <summary>
    /// Runs the cache algorithm and then determines whether or not to use the cache
    /// </summary>
    /// <returns></returns>
    public Tuple<WebsiteInfo, WebsiteInfoStatus> Get(string url, bool allowCache=true)
    {

        RequestEntry? prevEntry = null;

        prevEntry = DbCtx.ExtractorEntries
            .Where(x => x.Url == url)
            .OrderByDescending(entry => entry.Time)
            .FirstOrDefault();

        var ttRenew = CacheAlg.TimeToRenew(url, DbCtx.ExtractorEntries);
        var (cachedInfo, status) = GetFromCache(url, prevEntry, ttRenew);

        if (ttRenew > DateTime.Now && allowCache)
        {
            if (cachedInfo != null)
                return Tuple.Create(cachedInfo, status);
        }

        return GetFromExtractor(url, prevEntry, cachedInfo);
    }

    protected Tuple<WebsiteInfo?, WebsiteInfoStatus> GetFromCache(string url, RequestEntry? prevEntry, DateTime ttRenew)
    {
        WebsiteInfoStatus status = new();
        status.parsingDurationMS = 0;
        status.statusType = WebsiteInfoStatusType.cacheSuccess;

        DateTime startRequestTime = DateTime.Now;

        var info = DbCtx.WebsiteInfos
            .Where(x => x != null && x.Url == url)
            .FirstOrDefault();

        DateTime? cacheTime = null;

        if (prevEntry != null)
            cacheTime = prevEntry.Time;
        
        status.message = prevEntry != null ? $"From Cache ({cacheTime})" : "From Cache (Time unknown)";
        if (cacheTime != null)
        {
            DateTime upperRenewLimit = DateTime.Now.AddMonths(EmbyteStorage.upperLimitCacheAgeMonths);
            status.CacheDateInfo = Tuple.Create(cacheTime ?? DateTime.Now, ttRenew < upperRenewLimit ? ttRenew : upperRenewLimit);
        }

        TimeSpan requestDuration = DateTime.Now - startRequestTime;
        status.requestDurationMS = (int)Math.Round(requestDuration.TotalMilliseconds, MidpointRounding.AwayFromZero);

        return Tuple.Create(info, status);
    }

    protected Tuple<WebsiteInfo, WebsiteInfoStatus> GetFromExtractor(string url, RequestEntry? prevEntry, WebsiteInfo? cachedInfo)
    {
        TimeSpan deltaToPrev;
        bool dataChanged;

        var (info, status) = WebsiteInfoExtractor.GetMetaDataFromUrl(url);

        if (prevEntry == null)
        {
            deltaToPrev = new TimeSpan(0);
            dataChanged = false;
        } else
        {
            deltaToPrev = DateTime.Now.Subtract(prevEntry.Time);
            dataChanged = !info.DataEqual(cachedInfo);
        }

        RequestEntry entry = new(deltaToPrev, url, dataChanged);

        DbCtx.ExtractorEntries.Add(entry);

        if (cachedInfo == null)
        {
            DbCtx.WebsiteInfos.Add(info);
        }
        if (dataChanged && cachedInfo != null)
        {
            var cachedInfoEntry = DbCtx.WebsiteInfos.Where(i => i.Url == cachedInfo!.Url).First();
            cachedInfoEntry.SetPropertiesTo(info);
        }

        DbCtx.SaveChanges();

        return Tuple.Create(info, status);
    }
}
