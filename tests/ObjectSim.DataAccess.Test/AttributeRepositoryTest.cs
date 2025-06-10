using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ObjectSim.DataAccess.Repositories;

namespace ObjectSim.DataAccess.Test;
[TestClass]
public class AttributeRepositoryTest
{
    private SqliteConnection _connection = null!;
    private DbContext _context = null!;
    private AttributeRepository _attributeRepository = null!;

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

        _attributeRepository = new AttributeRepository(_context);
    }
    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }

    #region GetAttribute

    [TestMethod]
    public void GetAttribute_WhenIdExists_ReturnsAttributeWithDataType()
    {
        var attrId = Guid.NewGuid();
        var classId = Guid.NewGuid();
        var valueTypeId = Guid.NewGuid();

        var valueType = new Domain.ValueType("intValue", "int")
        {
            Id = valueTypeId
        };

        var classEntity = new Domain.Class
        {
            Id = classId,
            Name = "TestClass"
        };

        _context.Set<Domain.Class>().Add(classEntity);
        _context.Set<Domain.ValueType>().Add(valueType);
        _context.SaveChanges();

        var attribute = new Domain.Attribute
        {
            Id = attrId,
            Name = "TestAttribute",
            ClassId = classId,
            Visibility = Domain.Attribute.AttributeVisibility.Public,
            DataType = valueType
        };

        _context.Entry(attribute).Property("DataTypeId").CurrentValue = valueTypeId;

        _context.Set<Domain.Attribute>().Add(attribute);
        _context.SaveChanges();

        var result = _attributeRepository.Get(a => a.Id == attrId);

        Assert.IsNotNull(result);
        Assert.AreEqual(attrId, result.Id);
        Assert.IsNotNull(result.DataType);
        Assert.AreEqual("intValue", result.DataType.Name);
    }

    [TestMethod]
    public void GetAttribute_WhenIdDoesNotExist_ShouldReturnNull()
    {
        var result = _attributeRepository.Get(a => a.Id == Guid.NewGuid());

        Assert.IsNull(result);
    }

    [TestMethod]
    public void AddAttribute_ShouldPersistInDatabase()
    {
        var classId = Guid.NewGuid();
        var valueTypeId = Guid.NewGuid();

        var classEntity = new Domain.Class { Id = classId, Name = "TestClass" };
        var valueType = new Domain.ValueType("intValue", "int") { Id = valueTypeId };

        _context.Set<Domain.Class>().Add(classEntity);
        _context.Set<Domain.ValueType>().Add(valueType);
        _context.SaveChanges();

        var attribute = new Domain.Attribute
        {
            Id = Guid.NewGuid(),
            Name = "NewAttribute",
            ClassId = classId,
            Visibility = Domain.Attribute.AttributeVisibility.Public,
            DataType = valueType
        };

        _context.Entry(attribute).Property("DataTypeId").CurrentValue = valueTypeId;

        _attributeRepository.Add(attribute);

        var result = _attributeRepository.Get(a => a.Name == "NewAttribute");
        Assert.IsNotNull(result);
        Assert.AreEqual("NewAttribute", result.Name);
    }

    #endregion
}
