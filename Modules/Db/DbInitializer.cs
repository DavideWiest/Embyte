
namespace Embyte.Modules.Db;

using Microsoft.EntityFrameworkCore;

public class DbInitializer
{
    private readonly ModelBuilder modelBuilder;

    public DbInitializer(ModelBuilder modelBuilder)
    {
        this.modelBuilder = modelBuilder;
    }

    public void Seed()
    {
        //modelBuilder.Entity<User>().HasData(
        //       new User() { Id = 1.... },
        //       new User() { Id = 2.... },
        //);
    }
}
