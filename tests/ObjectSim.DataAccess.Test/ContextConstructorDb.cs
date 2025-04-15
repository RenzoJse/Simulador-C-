using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ObjectSim.DataAccess.Test;

internal static class ContextConstructorDb
{
    private static readonly SqliteConnection Conection = new("Data Source=:memory:");

    public static TestDbContext CreateMemoryContext()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseSqlite(Conection)
            .Options;

        Conection.Open();

        var context = new TestDbContext(options);

        return context;
    }
}
