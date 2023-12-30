using Embyte.Data.Product;
using Embyte.Modules.Db;
using System.Linq.Expressions;

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
    public Tuple<WebsiteInfo, WebsiteInfoStatus> Get(string url)
    {

        RequestEntry? prevEntry = null;

        prevEntry = DbCtx.ExtractorEntries
            .Where(x => x.Url == url)
            .OrderByDescending(entry => entry.Time)
            .FirstOrDefault();

        var (cachedInfo, status) = GetFromCache(url, prevEntry);

        if (CacheAlg.TimeToRenew(url, DbCtx.ExtractorEntries) > DateTime.Now)
        {
            if (cachedInfo != null)
                return Tuple.Create(cachedInfo, status);
        }

        return GetFromExtractor(url, prevEntry, cachedInfo);
    }

    protected Tuple<WebsiteInfo?, WebsiteInfoStatus> GetFromCache(string url, RequestEntry? prevEntry)
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

        if (dataChanged || prevEntry == null)
        {
            DbCtx.WebsiteInfos.Add(info);
        } else if (dataChanged)
        {
            // might not work
            DbCtx.WebsiteInfos.Entry(cachedInfo!).CurrentValues.SetValues(info);
        }

        DbCtx.SaveChanges();

        return Tuple.Create(info, status);
    }
}
