namespace Embyte.Modules.Db;

using Microsoft.EntityFrameworkCore;
using Embyte.Data.Models;


public class EmbyteDbContext : DbContext
{
    public DbSet<WebsiteUsage> WebsiteUsages { get; set; } = default!;

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

