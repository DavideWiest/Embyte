using Microsoft.EntityFrameworkCore;

namespace Embyte.Modules.Db;

public static class DbHelper
{
    public static bool CheckTableExists(TestDbContext dbContext, string tableName)
    {
        var sql = $"SELECT COUNT(*) FROM information_schema.tables WHERE table_name = '{tableName}'";
        var tableCount = dbContext.Database.SqlQuery<int>(sql).FirstOrDefault();
        return tableCount > 0;
    }

    public static List<string> GetExistingTables(TestDbContext dbContext)
    {
        var existingTables = new List<string>();

        var tables = dbContext.Database.SqlQuery<string>(
            "SELECT table_name FROM information_schema.tables WHERE table_type = 'BASE TABLE';"
        );

        existingTables.AddRange(tables);

        return existingTables;
        
    }

    public static int CheckNumberEntries(TestDbContext dbContext, string tableName)
    {
        var n = dbContext.Database.SqlQuery<int>(
            $"SELECT COUNT(*) FROM {tableName};"
        ).FirstOrDefault();

        return n;
    }
}
