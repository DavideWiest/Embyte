namespace Embyte.Modules.Db;

using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Embyte.Data.Models;


public class EmbyteDbContext : DbContext
{
    public DbSet<WebsiteUsage> WebsiteUsages { get; set; } = default!;

    public EmbyteDbContext(string ConnectionString) : base(ConnectionString)
    {
        
    }

    public EmbyteDbContext() : base(GetConnectionString())
    {
        
    }

    public static string GetConnectionString()
    {
        var configBuilder = new ConfigurationBuilder()
        .AddJsonFile("config/appsettings.json")
        .AddJsonFile("config/appsettings.Development.json")
        .Build();

#if DEBUG
        string ConnectionString = configBuilder["Database:ConnectionStringTesting"]!;
#else
        string ConnectionString  = configBuilder["Database:ConnectionStringProduction"]!;
#endif
        return ConnectionString;
    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<WebsiteUsage>().ToTable("WebsiteUsage");

        // seed content here
        new DbInitializer(modelBuilder).Seed();
    }
}

