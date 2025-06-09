

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ObjectSim.DataAccess.Repositories;
using ObjectSim.Domain;

namespace ObjectSim.DataAccess.Test;
[TestClass]
public class NamespaceRepositoryTest
{
    private SqliteConnection _connection = null!;
    private DataContext _context = null!;
    private NamespaceRepository _repository = null!;
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

        _repository = new NamespaceRepository(_context);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Dispose();
        _connection.Dispose();
    }
    [TestMethod]
    public void Add_ShouldPersistNamespace()
    {
        var ns = new Namespace { Name = "TestNamespace" };

        var result = _repository.Add(ns);

        var fromDb = _context.Namespaces.Find(result.Id);
        Assert.IsNotNull(fromDb);
        Assert.AreEqual("TestNamespace", fromDb!.Name);
    }
}
