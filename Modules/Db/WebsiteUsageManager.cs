using Embyte.Data.Models;

namespace Embyte.Modules.Db;
using Embyte.Modules.Logging;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

public class WebsiteUsageManager
{

    private EmbyteDbContext db = default!;

    public WebsiteUsageManager(EmbyteDbContext db)
    {
        this.db = db;
    }

    public bool InsertOne(string url)
    {
        try
        {
            WebsiteUsage newPerson = new WebsiteUsage { Url=url, RequestCount=1 };
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
            var existingWebsite = db.WebsiteUsages.FirstOrDefault(w => w.Url == url);
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

    public List<WebsiteUsage> ReadEntries(int limit=100, int offset = 0)
    {
        return db.WebsiteUsages
            .OrderBy(u => u.Id)  // Order by a unique column, e.g., Id
            .Skip(offset)
            .Take(limit)
            .ToList();
    }

    public async Task<List<WebsiteUsage>> ReadEntriesAsync(int limit = 100, int offset = 0)
    {
        // Use asynchronous methods for database access
        return await db.WebsiteUsages
            .OrderByDescending(u => u.RequestCount)  // Order by a unique column, e.g., Id
            .Skip(offset)
            .Take(limit)
            .ToListAsync(); // Use ToListAsync to asynchronously retrieve the data
    }
}
