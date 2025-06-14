using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ObjectSim.DataAccess.Repositories;

namespace ObjectSim.DataAccess.Test;

[TestClass]
public class MethodRepositoryTest
{
    private SqliteConnection _connection = null!;
    private DbContext _context = null!;
    private MethodRepository _methodRepository = null!;

    [TestInitialize]
    public void Setup()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<DataContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new DataContext(options);
        _context.Database.EnsureCreated();

        _methodRepository = new MethodRepository(_context);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
        _connection.Close();
    }

    [TestMethod]
    public void GetMethod_WhenIdExists_ReturnsMethodWithParametersAndMethodsInvoke()
    {
        // TODO
    }

}
