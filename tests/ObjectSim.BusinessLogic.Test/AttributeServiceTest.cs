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

    private readonly DataType? _testDataType = new ValueType(Guid.NewGuid(), "int");
    private static readonly Guid TestAttributeId = Guid.NewGuid();

    private readonly CreateAttributeArgs _testArgsAttribute = new(
        Guid.NewGuid(),
        "public",
        TestAttributeId,
        "Test",
        false
    );

    private readonly Attribute? _testAttribute = new()
    {
        Id = TestAttributeId,
        Name = "Test",
        DataType = new ValueType(Guid.NewGuid(), "int"),
        ClassId = Guid.NewGuid(),
        Visibility = Attribute.AttributeVisibility.Public
    };

    #region Setup & Cleanup

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

    #endregion

    #region Helpers

    private void SetupAttributeRepositoryGet(Attribute? attribute)
    {
        _attributeRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Attribute, bool>>()))
            .Returns(attribute);
    }

    private void SetupClassRepositoryGet(Class? @class)
    {
        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(@class);
    }

    private void SetupDataTypeServiceGetById(Guid id, DataType? dataType)
    {
        _dataTypeServiceMock!
            .Setup(x => x.GetById(id))
            .Returns(dataType!);
    }

    #endregion

    #region CreateAttribute

    #region Error

    [TestMethod]
    public void CreateAttribute_NotValidDataType_ThrowsException()
    {
        SetupDataTypeServiceGetById(_testArgsAttribute.DataTypeId, null);
        _dataTypeServiceMock
            .Setup(x => x.GetById(_testArgsAttribute.DataTypeId))
            .Throws(new ArgumentException("Invalid data type"));

        Action act = () => _attributeServiceTest!.CreateAttribute(_testArgsAttribute);

        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void CreateAttribute_InvalidVisibility_ThrowsException()
    {
        var invalidArgs = new CreateAttributeArgs(
            Guid.NewGuid(),
            "invalid_visibility",
            Guid.NewGuid(),
            "TestAttribute",
            false
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
        _testArgsAttribute.DataTypeId = Guid.NewGuid();

        SetupClassRepositoryGet(new Class { Id = _testAttribute.ClassId, Attributes = [] });
        SetupDataTypeServiceGetById(_testArgsAttribute.DataTypeId, _testDataType);
        _attributeRepositoryMock!
            .Setup(repo => repo.Add(It.IsAny<Attribute>()))
            .Returns((Attribute attr) => attr);

        var result = _attributeServiceTest!.CreateAttribute(_testArgsAttribute);
        result.Should().NotBeNull();
        result.Should().BeOfType<Attribute>();
        result.Should().BeEquivalentTo(_testAttribute, options =>
            options.Excluding(x => x.Id)
                .Excluding(x => x.DataTypeId)
                .Excluding(x => x.DataType)
                .Excluding(x => x.ClassId));
    }

    #endregion

    #endregion

    #region GetAll

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
                DataType = new ValueType(Guid.NewGuid(), "int"),
                Visibility = Attribute.AttributeVisibility.Private
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Name2",
                ClassId = Guid.NewGuid(),
                DataType = new ValueType(Guid.NewGuid(), "int"),
                Visibility = Attribute.AttributeVisibility.Private
            }
        };

        _attributeRepositoryMock!.Setup(repo => repo.GetAll(It.IsAny<Func<Attribute, bool>>()))
            .Returns(attributes);
        List<Attribute> result = _attributeServiceTest!.GetAll();
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

    #endregion

    #region Delete

    [TestMethod]
    public void DeleteAttribute_ValidId_ShouldCallDelete()
    {
        SetupAttributeRepositoryGet(_testAttribute);

        _attributeRepositoryMock!
            .Setup(repo => repo.Delete(It.IsAny<Attribute>()));

        _attributeServiceTest!.Delete(_testAttribute!.Id);

        _attributeRepositoryMock.Verify(repo => repo.Get(It.IsAny<Func<Attribute, bool>>()), Times.Once);
        _attributeRepositoryMock.Verify(repo => repo.Delete(It.IsAny<Attribute>()), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(KeyNotFoundException))]
    public void DeleteAttribute_InvalidId_ShouldThrowKeyNotFoundException()
    {
        SetupAttributeRepositoryGet(null);

        _attributeServiceTest!.Delete(Guid.Empty);
    }

    #endregion

    #region GetById

    [TestMethod]
    public void GetById_ValidId_ShouldReturnAttribute()
    {
        SetupAttributeRepositoryGet(_testAttribute);

        Attribute result = _attributeServiceTest!.GetById(_testAttribute!.Id);

        result.Should().NotBeNull();
        result.Should().Be(_testAttribute);

        _attributeRepositoryMock!.Verify(repo => repo.Get(It.IsAny<Func<Attribute, bool>>()), Times.Once);
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
        SetupAttributeRepositoryGet(null);

        _attributeServiceTest!.GetById(Guid.NewGuid());
    }

    #endregion

    #region Update

    [TestMethod]
    public void UpdateAttribute_ValidAttribute_ShouldUpdateAndReturnAttribute()
    {
        var args = new CreateAttributeArgs(
            Guid.NewGuid(),
            "Public",
            Guid.NewGuid(),
            "UpdatedName",
            false
        );

        var updatedAttribute = new Attribute
        {
            Id = Guid.NewGuid(),
            Name = "UpdatedName",
            ClassId = args.ClassId,
            Visibility = Attribute.AttributeVisibility.Public,
            DataType = new ReferenceType(Guid.NewGuid(), "Reference")
        };

        SetupAttributeRepositoryGet(updatedAttribute);

        _attributeRepositoryMock!
            .Setup(repo => repo.Update(It.IsAny<Attribute>()))
            .Returns((Attribute attr) => attr);

        SetupClassRepositoryGet(new Class { Id = args.ClassId });
        SetupDataTypeServiceGetById(args.DataTypeId, updatedAttribute.DataType);

        var result = _attributeServiceTest!.Update(updatedAttribute.Id, args);

        result.Should().NotBeNull();
        result.Name.Should().Be("UpdatedName");

        _attributeRepositoryMock.Verify(repo => repo.Get(It.IsAny<Func<Attribute, bool>>()), Times.Once);
        _attributeRepositoryMock.Verify(repo => repo.Update(It.IsAny<Attribute>()), Times.Once);
        _classRepositoryMock.Verify(repo => repo.Get(It.IsAny<Func<Class, bool>>()), Times.Once);
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
        SetupAttributeRepositoryGet(null);

        var id = Guid.NewGuid();
        var createArgs = new CreateAttributeArgs(
            Guid.NewGuid(),
            "Public",
            Guid.NewGuid(),
            "Attr",
            false
        );

        _attributeServiceTest!.Update(id, createArgs);
    }

    [TestMethod]
    [ExpectedException(typeof(KeyNotFoundException))]
    public void UpdateAttribute_ClassDoesNotExist_ShouldThrowKeyNotFoundException()
    {
        var validAttribute = new Attribute
        {
            Id = Guid.NewGuid(),
            Name = "Attr",
            ClassId = Guid.NewGuid(),
            Visibility = Attribute.AttributeVisibility.Public,
            DataType = new ValueType(Guid.NewGuid(), "int")
        };

        var args = new CreateAttributeArgs(
            Guid.NewGuid(),
            "Public",
            validAttribute.ClassId,
            validAttribute.Name,
            false
        );

        SetupAttributeRepositoryGet(validAttribute);
        SetupClassRepositoryGet(null);

        _attributeServiceTest!.Update(validAttribute.Id, args);
    }

    #endregion

    #region GetByClassId

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

    #endregion
}
