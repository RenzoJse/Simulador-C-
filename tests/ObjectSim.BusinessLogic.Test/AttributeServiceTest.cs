using FluentAssertions;
using Moq;
using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;
using Attribute = ObjectSim.Domain.Attribute;

namespace ObjectSim.BusinessLogic.Test;
[TestClass]
public class AttributeServiceTest
{
    private Mock<IRepository<Attribute>>? _attributeRepositoryMock;
    private Mock<IClassService> _classServiceMock = null!;
    private Mock<IDataTypeService> _dataTypeServiceMock = null!;
    private AttributeService? _attributeService;
    private Attribute? _attribute;

    private static CreateDataTypeArgs _testArgsDataType = new CreateDataTypeArgs(
        "int");

    private CreateAttributeArgs _testArgsAttribute = new CreateAttributeArgs(
        _testArgsDataType,
        "public",
        Guid.NewGuid(),
        "Test"
    );

    private readonly IDataType? _testDataType = ReferenceType.Create("int");

    [TestInitialize]
    public void Setup()
    {
        _classServiceMock = new Mock<IClassService>();
        _dataTypeServiceMock = new Mock<IDataTypeService>();
        _attributeRepositoryMock = new Mock<IRepository<Attribute>>(MockBehavior.Strict);
        _attributeService = new AttributeService(_attributeRepositoryMock.Object, _classServiceMock.Object, _dataTypeServiceMock.Object);

        _attribute = new Attribute
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            DataType = Domain.ValueType.Create("int"),
            ClassId = Guid.NewGuid(),
            Visibility = Attribute.AttributeVisibility.Public
        };
    }

    [TestCleanup]
    public void Cleanup()
    {
        _attribute = null;
        _attributeRepositoryMock!.VerifyAll();
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
        _classServiceMock.Setup(x => x.CanAddAttribute(It.IsAny<Class>(), It.IsAny<Attribute>())).Returns(false);
        Action act = () => _attributeService!.GetAll();

        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void CreateAttribute_NotValidDataType_ThrowsException()
    {
        _classServiceMock.Setup(x => x.CanAddAttribute(It.IsAny<Class>(), It.IsAny<Attribute>())).Returns(true);
        _dataTypeServiceMock.Setup(x => x.CreateDataType(_testArgsAttribute.DataType)).Returns(_testDataType!);
        Action act = () => _attributeService!.CreateAttribute(_testArgsAttribute);

        act.Should().Throw<ArgumentException>();
    }

    #endregion

    #region Success

    [TestMethod]
    public void CreateAttribute_WithValidParameters_ReturnsNewAttribute()
    {
        _attributeRepositoryMock!
            .Setup(repo => repo.Add(It.IsAny<Attribute>()))
            .Returns(_attribute!);

        _attributeRepositoryMock.Verify(repo => repo.Add(It.IsAny<Attribute>()), Times.Once);
    }

    #endregion

    #endregion

    [TestMethod]
    public void GetAllAttribute_CorrectAttributes_ShouldThrowAllOfThem()
    {
        var attributes = new List<Attribute>
        {
            new Attribute
        {
            Id = Guid.NewGuid(),
            Name = "Name",
            ClassId = Guid.NewGuid(),
            DataType = Domain.ValueType.Create("int"),
            Visibility = Attribute.AttributeVisibility.Private
        },
            new Attribute
        {
            Id = Guid.NewGuid(),
            Name = "Name2",
            ClassId = Guid.NewGuid(),
            DataType = Domain.ValueType.Create("int"),
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
            .Returns(_attribute);

        _attributeRepositoryMock!
            .Setup(repo => repo.Delete(It.IsAny<Attribute>()));

        _attributeService!.Delete(_attribute!.Id);

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
            .Returns(_attribute);

        var result = _attributeService!.GetById(_attribute!.Id);

        result.Should().NotBeNull();
        result.Should().Be(_attribute);

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
        _attribute!.Name = "UpdatedName";
        _attributeRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Attribute, bool>>()))
            .Returns(_attribute);

        _attributeRepositoryMock!
            .Setup(repo => repo.Update(It.IsAny<Attribute>()))
            .Returns((Attribute attr) => attr);

        var result = _attributeService!.Update(_attribute.Id, _attribute);

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
            DataType = Domain.ValueType.Create("int")
        };

        _attributeService!.Update(dummyAttribute.Id, dummyAttribute);
    }
}
