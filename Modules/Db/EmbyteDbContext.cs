namespace Embyte.Modules.Db;

using Microsoft.EntityFrameworkCore;
using Embyte.Data.Models;
using Embyte.Data.Product;

public class EmbyteDbContext : DbContext
{
    public DbSet<WebsiteUsage> WebsiteUsages { get; set; } = default!;
    public DbSet<ExtractorEntry> ExtractorEntries { get; set; } = default!;
    public DbSet<WebsiteInfo> WebsiteInfos { get; set; } = default!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(GetConnectionString());
    }

    private static string GetConnectionString()
    {
#if DEBUG
        return Environment.GetEnvironmentVariable("Embyte_Database_ConnectionStringDevelopment")!;
#else
        return Environment.GetEnvironmentVariable("Embyte_Database_ConnectionStringProduction")!;
#endif
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<WebsiteUsage>().ToTable("WebsiteUsage");

        // seed content here
        new DbInitializer(modelBuilder).Seed();
    }
}

