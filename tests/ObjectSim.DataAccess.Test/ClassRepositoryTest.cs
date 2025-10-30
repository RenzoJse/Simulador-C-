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
        _context.Dispose();
        _connection.Close();
    }

    [TestMethod]
    public void GetClass_WhenIdExists_ReturnsClassWithMethodsAndAttributes()
    {
        var classId = Guid.NewGuid();
        var valueType = new ValueType(Guid.NewGuid(), "int");
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

    [TestMethod]
    public void GetAll_ReturnsAllClassesWithMethodsAndAttributes()
    {
        var valueType = new ValueType(Guid.NewGuid(), "int");
        _context.Set<ValueType>().Add(valueType);
        _context.SaveChanges();

        var class1Id = Guid.NewGuid();
        var class2Id = Guid.NewGuid();

        var class1 = new Class
        {
            Id = class1Id,
            Name = "Class1",
            Methods = [new Method { Id = Guid.NewGuid(), Name = "M1", ClassId = class1Id, TypeId = valueType.Id }],
            Attributes = [new Attribute { Id = Guid.NewGuid(), Name = "A1", ClassId = class1Id, DataType = valueType, Visibility = 0 }]
        };
        var class2 = new Class
        {
            Id = class2Id,
            Name = "Class2",
            Methods = [new Method { Id = Guid.NewGuid(), Name = "M2", ClassId = class2Id, TypeId = valueType.Id }],
            Attributes = [new Attribute { Id = Guid.NewGuid(), Name = "A2", ClassId = class2Id, DataType = valueType, Visibility = 0 }]
        };

        _context.Set<Class>().AddRange(class1, class2);
        _context.SaveChanges();

        var result = _classRepository.GetAll(_ => true);
        var ids = new[] { class1Id, class2Id };
        var filtered = result.Where(c => ids.Contains(c.Id)).ToList();

        Assert.AreEqual(2, filtered.Count);
        Assert.IsTrue(filtered.All(c => c.Methods!.Count == 1));
        Assert.IsTrue(filtered.All(c => c.Attributes!.Count == 1));
    }

}
