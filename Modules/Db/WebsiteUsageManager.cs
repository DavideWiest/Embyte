using Embyte.Data.Models;

namespace Embyte.Modules.Db;
using Embyte.Modules.Logging;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

public class WebsiteUsageManager
{

    private EmbyteDbContext db;

    public WebsiteUsageManager(EmbyteDbContext db)
    {
        this.db = db;
    }

    public bool InsertOne(string url)
    {
        try
        {
            WebsiteUsage newPerson = new WebsiteUsage { Url = url, RequestCount = 1 };
            db.WebsiteUsages.Add(newPerson);
            db.SaveChanges();
            return true;
        }
        catch (Exception e) {
            Log.Error("Unable to execute InsertOne for the url {url}: {errMsg}", url, e.Message);
            return false;
        }
    }

    public bool SaveRequest(string url)
    {
        try
        {
            // Check if the URL already exists in the database
            var existingWebsite = db.WebsiteUsages.ToList().FirstOrDefault(w => w.Url == url);
            if (existingWebsite != null)
            {
                // If the URL exists, increment the RequestCount
                existingWebsite.RequestCount++;
            }
            else
            {
                // If the URL doesn't exist, create a new record with RequestCount = 1
                WebsiteUsage newWebsite = new WebsiteUsage { Url = url, RequestCount = 1 };
                db.WebsiteUsages.Add(newWebsite);
            }
            db.SaveChanges();
            return true;
        }
        catch (Exception e)
        {
            Log.Error("Unable to execute SaveRequest for the url {url}: {errMsg}", url, e);
            return false;
        }
    }

    public List<WebsiteUsage> ReadEntries(int limit = 100, int offset = 0)
    {
        int entryCount = db.WebsiteUsages.Count();
        if (offset > entryCount)
        {
            return new();
        }
        limit = Math.Min(limit, entryCount-offset-1);

        Console.WriteLine(limit);
        Console.WriteLine(offset);
        Console.WriteLine(entryCount);

        return db.WebsiteUsages
            .OrderByDescending(u => u.RequestCount)
            .Skip(offset)
            .Take(limit)
            .ToList();
    }

    public int GetRequestCount(string url)
    {
        var entry = db.WebsiteUsages.FirstOrDefault(w => w.Url == url);
        int requestCount = 1;
        if (entry != null) {
            requestCount = entry.RequestCount;
        }
        return requestCount;
    }

    public int CountUsageEntries()
    {
        return db.WebsiteUsages.Count();
    }

    public Tuple<int, int> CountTotalStats()
    {
        int websitesTotal = db.WebsiteUsages.Count();
        int requestsTotal = db.WebsiteUsages.Select(u => u.RequestCount).Sum();

        return Tuple.Create(websitesTotal, requestsTotal);
    }
}