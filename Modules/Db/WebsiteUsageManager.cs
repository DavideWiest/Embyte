using Embyte.Data.Models;

namespace Embyte.Modules.Db;

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
            WebsiteUsage newPerson = new WebsiteUsage { Url=url };
            db.WebsiteUsages.Add(newPerson);
            db.SaveChanges();
            return true;
        }
        catch { return false; }
    }

    public List<WebsiteUsage> ReadAll()
    {
        return db.WebsiteUsages.ToList();
    }
}
