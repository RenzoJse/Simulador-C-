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
    private Mock<IClassService> _classServiceMock = null!;
    private Mock<IDataTypeService> _dataTypeServiceMock = null!;
    private AttributeService? _attributeService;

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
        _classServiceMock = new Mock<IClassService>();
        _dataTypeServiceMock = new Mock<IDataTypeService>();
        _attributeRepositoryMock = new Mock<IRepository<Attribute>>(MockBehavior.Strict);
        _attributeService = new AttributeService(_attributeRepositoryMock.Object, _classServiceMock.Object, _dataTypeServiceMock.Object);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _attributeRepositoryMock!.VerifyAll();
        _classServiceMock.VerifyAll();
        _dataTypeServiceMock.VerifyAll();
    }

    #region CreateAttribute

    #region Error

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CreateAttribute_NullArgs_ThrowsException()
    {
        _attributeService!.CreateAttribute(null!);
    }

    [TestMethod]
    public void CreateAttribute_AttributeWithSameNameAlreadyExistsInClass_ThrowsException()
    {
        _dataTypeServiceMock.Setup(x => x.CreateDataType(TestArgsDataType)).Returns(_testDataType!);
        _classServiceMock
            .Setup(x => x.AddAttribute(It.IsAny<Guid>(), It.IsAny<Attribute>()))
            .Throws(new ArgumentException());

        Action act = () => _attributeService!.CreateAttribute(_testArgsAttribute);

        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void CreateAttribute_NotValidDataType_ThrowsException()
    {
        _dataTypeServiceMock
            .Setup(x => x.CreateDataType(_testArgsAttribute.DataType))
            .Throws(new ArgumentException("Invalid data type"));

        Action act = () => _attributeService!.CreateAttribute(_testArgsAttribute);

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

        Action act = () => _attributeService!.CreateAttribute(invalidArgs);

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

        _dataTypeServiceMock
            .Setup(x => x.CreateDataType(_testArgsAttribute.DataType)).Returns(_testDataType!);

        _classServiceMock.Setup(x => x.AddAttribute(It.IsAny<Guid>(), It.IsAny<Attribute>()));

        _attributeRepositoryMock!
            .Setup(repo => repo.Add(It.IsAny<Attribute>()))
            .Returns(_testAttribute!);

        var result = _attributeService!.CreateAttribute(_testArgsAttribute);
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
        Assert.IsNotNull(_attributeService);
        _attributeRepositoryMock.Setup(repo => repo.GetAll(It.IsAny<Func<Attribute, bool>>()))
            .Returns(attributes);
        var result = _attributeService.GetAll();
        result.Should().HaveCount(2);
        _attributeRepositoryMock.Verify(repo => repo.GetAll(It.IsAny<Func<Attribute, bool>>()), Times.Once);
    }
    [TestMethod]
    public void GetAll_ShouldThrowException_WhenRepositoryReturnsEmptyList()
    {
        _attributeRepositoryMock!
            .Setup(repo => repo.GetAll(It.IsAny<Func<Attribute, bool>>()))
            .Returns([]);

        Action act = () => _attributeService!.GetAll();

        act.Should().Throw<Exception>().WithMessage("No attributes found.");
        _attributeRepositoryMock.Verify(repo => repo.GetAll(It.IsAny<Func<Attribute, bool>>()), Times.Once);
    }
    [TestMethod]
    public void GetAll_ShouldThrowException_WhenRepositoryReturnsNull()
    {
        _attributeRepositoryMock!
            .Setup(repo => repo.GetAll(It.IsAny<Func<Attribute, bool>>()))
            .Returns((List<Attribute>)null!);

        Action act = () => _attributeService!.GetAll();

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

        _attributeService!.Delete(_testAttribute!.Id);

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

        _attributeService!.Delete(Guid.Empty);
    }
    [TestMethod]
    public void GetById_ValidId_ShouldReturnAttribute()
    {
        _attributeRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Attribute, bool>>()))
            .Returns(_testAttribute);

        var result = _attributeService!.GetById(_testAttribute!.Id);

        result.Should().NotBeNull();
        result.Should().Be(_testAttribute);

        _attributeRepositoryMock.Verify(repo => repo.Get(It.IsAny<Func<Attribute, bool>>()), Times.Once);
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void GetById_InvalidId_ShouldThrowArgumentException()
    {
        _attributeService!.GetById(Guid.Empty);
    }
    [TestMethod]
    [ExpectedException(typeof(KeyNotFoundException))]
    public void GetById_NonExistentId_ShouldThrowKeyNotFoundException()
    {
        _attributeRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Attribute, bool>>()))
            .Returns((Attribute)null!);

        _attributeService!.GetById(Guid.NewGuid());
    }
    [TestMethod]
    public void UpdateAttribute_ValidAttribute_ShouldUpdateAndReturnAttribute()
    {
        _testAttribute!.Name = "UpdatedName";
        _attributeRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Attribute, bool>>()))
            .Returns(_testAttribute);

        _attributeRepositoryMock!
            .Setup(repo => repo.Update(It.IsAny<Attribute>()))
            .Returns((Attribute attr) => attr);

        var result = _attributeService!.Update(_testAttribute.Id, _testAttribute);

        result.Should().NotBeNull();
        result.Name.Should().Be("UpdatedName");

        _attributeRepositoryMock.Verify(repo => repo.Get(It.IsAny<Func<Attribute, bool>>()), Times.Once);
        _attributeRepositoryMock.Verify(repo => repo.Update(It.IsAny<Attribute>()), Times.Once);
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void UpdateAttribute_NullAttribute_ShouldThrowArgumentNullException()
    {
        _attributeService!.Update(Guid.NewGuid(), null!);
    }
    [TestMethod]
    [ExpectedException(typeof(KeyNotFoundException))]
    public void UpdateAttribute_NonExistentAttribute_ShouldThrowKeyNotFoundException()
    {
        _attributeRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Attribute, bool>>()))
            .Returns((Attribute)null!);

        var dummyAttribute = new Attribute
        {
            Id = Guid.NewGuid(),
            Name = "Attr",
            ClassId = Guid.NewGuid(),
            Visibility = Attribute.AttributeVisibility.Public,
            DataType = new ValueType("myVariable", "int", [])
        };

        _attributeService!.Update(dummyAttribute.Id, dummyAttribute);
    }
    [TestMethod]
    public void GetByClassId_ShouldReturnAttributes()
    {
        _attributeRepositoryMock.Setup(r => r.GetAll(It.IsAny<Func<Attribute, bool>>()))
            .Returns([_testAttribute]);

        var result = _attributeService.GetByClassId(_testAttribute.ClassId);

        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual("Test", result[0].Name);
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void GetByClassId_ShouldThrow_WhenClassIdIsEmpty()
    {
        _attributeService.GetByClassId(Guid.Empty);
    }
    [TestMethod]
    [ExpectedException(typeof(KeyNotFoundException))]
    public void GetByClassId_ShouldThrow_WhenNoAttributesFound()
    {
        _attributeRepositoryMock.Setup(r => r.GetAll(It.IsAny<Func<Attribute, bool>>()))
            .Returns([]);

        _attributeService.GetByClassId(_testAttribute.ClassId);
    }
}
