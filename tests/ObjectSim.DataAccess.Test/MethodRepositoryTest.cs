using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ObjectSim.DataAccess.Repositories;
using ObjectSim.Domain;

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
    public void GetMethod_WhenIdExists_ReturnsMethod()
    {
        var classEntity = new Class
        {
            Id = Guid.NewGuid(),
            Name = "TestClass"
        };
        _context.Set<Class>().Add(classEntity);
        _context.SaveChanges();

        var voidTypeId = Guid.Parse("00000000-0000-0000-0000-000000000005");

        var method = new Method
        {
            Id = Guid.NewGuid(),
            Name = "TestMethod",
            ClassId = classEntity.Id,
            TypeId = voidTypeId
        };

        _context.Set<Method>().Add(method);
        _context.SaveChanges();

        var result = _methodRepository.Get(m => m.Id == method.Id);

        Assert.IsNotNull(result);
        Assert.AreEqual("TestMethod", result.Name);
    }
}

