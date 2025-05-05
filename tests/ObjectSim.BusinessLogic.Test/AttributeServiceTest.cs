using FluentAssertions;
using Moq;
using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;
using Attribute = ObjectSim.Domain.Attribute;
using ValueType = ObjectSim.Domain.ValueType;

namespace ObjectSim.BusinessLogic.Test;
[TestClass]
public class AttributeServiceTest
{
    private Mock<IRepository<Attribute>>? _attributeRepositoryMock;
    private Mock<IRepository<Class>>? _classRepositoryMock;
    private Mock<IDataTypeService> _dataTypeServiceMock = null!;
    private AttributeService? _attributeServiceTest;

    private static readonly CreateDataTypeArgs TestArgsDataType = new(
        "int", "reference");

    private readonly DataType? _testDataType = new ValueType("myVariable", "int", []);

    private static readonly Guid TestAttributeId = Guid.NewGuid();

    private readonly CreateAttributeArgs _testArgsAttribute = new(
        TestArgsDataType,
        "public",
        TestAttributeId,
        "Test"
    );

    private readonly Attribute? _testAttribute = new()
    {
        Id = TestAttributeId,
        Name = "Test",
        DataType = new ValueType("myVariable", "int", []),
        ClassId = Guid.NewGuid(),
        Visibility = Attribute.AttributeVisibility.Public
    };

    [TestInitialize]
    public void Setup()
    {
        _dataTypeServiceMock = new Mock<IDataTypeService>();
        _attributeRepositoryMock = new Mock<IRepository<Attribute>>(MockBehavior.Strict);
        _classRepositoryMock = new Mock<IRepository<Class>>(MockBehavior.Strict);
        _attributeServiceTest = new AttributeService(_attributeRepositoryMock.Object, _classRepositoryMock.Object, _dataTypeServiceMock.Object);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _attributeRepositoryMock!.VerifyAll();
        _classRepositoryMock!.VerifyAll();
        _dataTypeServiceMock.VerifyAll();
    }

    #region CreateAttribute

    #region Error

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CreateAttribute_NullArgs_ThrowsException()
    {
        _attributeServiceTest!.CreateAttribute(null!);
    }

    [TestMethod]
    public void CreateAttribute_NotValidDataType_ThrowsException()
    {
        _dataTypeServiceMock
            .Setup(x => x.CreateDataType(_testArgsAttribute.DataType))
            .Throws(new ArgumentException("Invalid data type"));

        Action act = () => _attributeServiceTest!.CreateAttribute(_testArgsAttribute);

        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void CreateAttribute_InvalidVisibility_ThrowsException()
    {
        var invalidArgs = new CreateAttributeArgs(
            new CreateDataTypeArgs("int", "value"),
            "invalid_visibility",
            Guid.NewGuid(),
            "TestAttribute"
        );

        Action act = () => _attributeServiceTest!.CreateAttribute(invalidArgs);

        act.Should().Throw<ArgumentException>()
            .WithMessage("Invalid visibility value: invalid_visibility");
    }

    #endregion

    #region Success

    [TestMethod]
    public void CreateAttribute_WithValidParameters_ReturnsNewAttribute()
    {
        _testArgsAttribute.ClassId = _testAttribute!.ClassId;
        _testArgsAttribute.Id = _testAttribute.Id;
        _testArgsAttribute.Name = _testAttribute.Name!;
        _testArgsAttribute.Visibility = _testAttribute.Visibility.ToString();
        _testArgsAttribute.DataType = TestArgsDataType;

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(new Class { Id = _testAttribute.ClassId, Attributes = [] });

        _dataTypeServiceMock
            .Setup(x => x.CreateDataType(_testArgsAttribute.DataType)).Returns(_testDataType!);

        _attributeRepositoryMock!
            .Setup(repo => repo.Add(It.IsAny<Attribute>()))
            .Returns(_testAttribute!);

        var result = _attributeServiceTest!.CreateAttribute(_testArgsAttribute);
        result.Should().NotBeNull();
        result.Should().BeOfType<Attribute>();
        result.Should().BeEquivalentTo(_testAttribute, options =>
            options.Excluding(x => x.DataType.Id));
    }

    #endregion

    #endregion

    [TestMethod]
    public void GetAllAttribute_CorrectAttributes_ShouldThrowAllOfThem()
    {
        var attributes = new List<Attribute>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                ClassId = Guid.NewGuid(),
                DataType = new ValueType("myVariable", "int", []),
                Visibility = Attribute.AttributeVisibility.Private
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Name2",
                ClassId = Guid.NewGuid(),
                DataType = new ValueType("myVariable", "int", []),
                Visibility = Attribute.AttributeVisibility.Private
            }
        };

        Assert.IsNotNull(_attributeRepositoryMock);
        Assert.IsNotNull(_attributeServiceTest);
        _attributeRepositoryMock.Setup(repo => repo.GetAll(It.IsAny<Func<Attribute, bool>>()))
            .Returns(attributes);
        List<Attribute> result = _attributeServiceTest.GetAll();
        result.Should().HaveCount(2);
        _attributeRepositoryMock.Verify(repo => repo.GetAll(It.IsAny<Func<Attribute, bool>>()), Times.Once);
    }

    [TestMethod]
    public void GetAll_ShouldThrowException_WhenRepositoryReturnsEmptyList()
    {
        _attributeRepositoryMock!
            .Setup(repo => repo.GetAll(It.IsAny<Func<Attribute, bool>>()))
            .Returns([]);

        Action act = () => _attributeServiceTest!.GetAll();

        act.Should().Throw<Exception>().WithMessage("No attributes found.");
        _attributeRepositoryMock.Verify(repo => repo.GetAll(It.IsAny<Func<Attribute, bool>>()), Times.Once);
    }

    [TestMethod]
    public void GetAll_ShouldThrowException_WhenRepositoryReturnsNull()
    {
        _attributeRepositoryMock!
            .Setup(repo => repo.GetAll(It.IsAny<Func<Attribute, bool>>()))
            .Returns((List<Attribute>)null!);

        Action act = () => _attributeServiceTest!.GetAll();

        act.Should().Throw<Exception>()
            .WithMessage("No attributes found.");

        _attributeRepositoryMock.Verify(repo => repo.GetAll(It.IsAny<Func<Attribute, bool>>()), Times.Once);
    }

    [TestMethod]
    public void DeleteAttribute_ValidId_ShouldCallDelete()
    {
        _attributeRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Attribute, bool>>()))
            .Returns(_testAttribute);

        _attributeRepositoryMock!
            .Setup(repo => repo.Delete(It.IsAny<Attribute>()));

        _attributeServiceTest!.Delete(_testAttribute!.Id);

        _attributeRepositoryMock.Verify(repo => repo.Get(It.IsAny<Func<Attribute, bool>>()), Times.Once);
        _attributeRepositoryMock.Verify(repo => repo.Delete(It.IsAny<Attribute>()), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void DeleteAttribute_InvalidId_ShouldThrowException()
    {
        _attributeRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Attribute, bool>>()))
            .Returns((Attribute)null!);

        _attributeServiceTest!.Delete(Guid.Empty);
    }

    [TestMethod]
    public void GetById_ValidId_ShouldReturnAttribute()
    {
        _attributeRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Attribute, bool>>()))
            .Returns(_testAttribute);

        Attribute result = _attributeServiceTest!.GetById(_testAttribute!.Id);

        result.Should().NotBeNull();
        result.Should().Be(_testAttribute);

        _attributeRepositoryMock.Verify(repo => repo.Get(It.IsAny<Func<Attribute, bool>>()), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void GetById_InvalidId_ShouldThrowArgumentException()
    {
        _attributeServiceTest!.GetById(Guid.Empty);
    }

    [TestMethod]
    [ExpectedException(typeof(KeyNotFoundException))]
    public void GetById_NonExistentId_ShouldThrowKeyNotFoundException()
    {
        _attributeRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Attribute, bool>>()))
            .Returns((Attribute)null!);

        _attributeServiceTest!.GetById(Guid.NewGuid());
    }
    [TestMethod]
    public void UpdateAttribute_ValidAttribute_ShouldUpdateAndReturnAttribute()
    {
        var args = new CreateAttributeArgs(
            new CreateDataTypeArgs("stringName", "Reference"),
            "Public",
            Guid.NewGuid(),
            "UpdatedName"
        );

        var updatedAttribute = new Attribute
        {
            Id = Guid.NewGuid(),
            Name = "UpdatedName",
            ClassId = args.ClassId,
            Visibility = Attribute.AttributeVisibility.Public,
            DataType = new ReferenceType("stringName", "Reference", [])
        };

        _attributeRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Attribute, bool>>()))
            .Returns(updatedAttribute);

        _attributeRepositoryMock!
            .Setup(repo => repo.Update(It.IsAny<Attribute>()))
            .Returns((Attribute attr) => attr);

        var result = _attributeServiceTest!.Update(updatedAttribute.Id, args);

        result.Should().NotBeNull();
        result.Name.Should().Be("UpdatedName");

        _attributeRepositoryMock.Verify(repo => repo.Get(It.IsAny<Func<Attribute, bool>>()), Times.Once);
        _attributeRepositoryMock.Verify(repo => repo.Update(It.IsAny<Attribute>()), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void UpdateAttribute_NullAttribute_ShouldThrowArgumentNullException()
    {
        _attributeServiceTest!.Update(Guid.NewGuid(), null!);
    }
    [TestMethod]
    [ExpectedException(typeof(KeyNotFoundException))]
    public void UpdateAttribute_NonExistentAttribute_ShouldThrowKeyNotFoundException()
    {
        _attributeRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Attribute, bool>>()))
            .Returns((Attribute)null!);

        var id = Guid.NewGuid();
        var createArgs = new CreateAttributeArgs(
            new CreateDataTypeArgs("myVariable", "Value"),
            "Public",
            Guid.NewGuid(),
            "Attr"
        );

        _attributeServiceTest!.Update(id, createArgs);
    }
    [TestMethod]
    public void GetByClassId_ShouldReturnAttributes()
    {
        _attributeRepositoryMock!.Setup(r => r.GetAll(It.IsAny<Func<Attribute, bool>>()))
            .Returns([_testAttribute!]);

        List<Attribute> result = _attributeServiceTest!.GetByClassId(_testAttribute!.ClassId);

        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual("Test", result[0].Name);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void GetByClassId_ShouldThrow_WhenClassIdIsEmpty()
    {
        _attributeServiceTest!.GetByClassId(Guid.Empty);
    }

    [TestMethod]
    [ExpectedException(typeof(KeyNotFoundException))]
    public void GetByClassId_ShouldThrow_WhenNoAttributesFound()
    {
        _attributeRepositoryMock!.Setup(r => r.GetAll(It.IsAny<Func<Attribute, bool>>()))
            .Returns([]);

        _attributeServiceTest!.GetByClassId(_testAttribute!.ClassId);
    }
}
