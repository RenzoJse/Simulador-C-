using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ObjectSim.DataAccess.Repositories;
using ObjectSim.Domain;
using Attribute = ObjectSim.Domain.Attribute;
using ValueType = ObjectSim.Domain.ValueType;

namespace ObjectSim.DataAccess.Test;

[TestClass]
public class ClassRepositoryTest
{
    private SqliteConnection _connection = null!;
    private DbContext _context = null!;
    private ClassRepository _classRepository = null!;

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

        _classRepository = new ClassRepository(_context);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }

    [TestMethod]
    public void GetClass_WhenIdExists_ReturnsClassWithMethodsAndAttributes()
    {
    var classId = Guid.NewGuid();
    var valueType = new ValueType("myVariable", "int", []);
    _context.Set<ValueType>().Add(valueType);
    _context.SaveChanges();

    var classEntity = new Class
    {
        Id = classId,
        Name = "TestClass",
        Methods =
        [
            new Method
            {
                Id = Guid.NewGuid(),
                Name = "TestMethod",
                ClassId = classId,
                Accessibility = 0,
                Abstract = false,
                IsOverride = false,
                IsSealed = false,
                TypeId = valueType.Id
            }
        ],
        Attributes =
        [
            new Attribute
            {
                Id = Guid.NewGuid(),
                Name = "TestAttribute",
                ClassId = classId,
                DataType = valueType,
                Visibility = 0
            }
        ]
    };

    _context.Set<Class>().Add(classEntity);
    _context.SaveChanges();

    var result = _classRepository.Get(c => c.Id == classId);

    Assert.IsNotNull(result);
    Assert.AreEqual(classId, result.Id);
    Assert.AreEqual(1, result.Methods!.Count);
    Assert.AreEqual(1, result.Attributes!.Count);
    }

}
