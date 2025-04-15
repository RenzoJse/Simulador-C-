using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ObjectSim.DataAccess.Test;

internal sealed class ContextConstructorDB
{
    private static readonly SqliteConnection _conection = new("Data Source=:memory:");

    public static TestDbContext CreateMemoryContext()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseSqlite(_conection)
            .Options;

        _conection.Open();

        var context = new TestDbContext(options);

        return context;
    }
}
