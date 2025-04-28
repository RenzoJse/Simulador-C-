using FluentAssertions;
using Moq;
using ObjectSim.DataAccess.Interface;

namespace ObjectSim.BusinessLogic.Test;
[TestClass]
public class AttributeServiceTest
{
    private Mock<IRepository<Domain.Attribute>>? _attributeRepositoryMock;
    private AttributeService? _service;
    private ObjectSim.Domain.Attribute? _attribute;

    [TestInitialize]
    public void Setup()
    {
        _attributeRepositoryMock = new Mock<IRepository<Domain.Attribute>>(MockBehavior.Strict);
        _service = new AttributeService(_attributeRepositoryMock.Object);

        _attribute = new ObjectSim.Domain.Attribute
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            DataType = Domain.ValueType.Create("int"),
            ClassId = Guid.NewGuid(),
            Visibility = Domain.Attribute.AttributeVisibility.Public
        };
    }

    [TestCleanup]
    public void Cleanup()
    {
        _attribute = null;
    }

    public ObjectSim.Domain.Attribute? Get_attribute()
    {
        return _attribute;
    }

    [TestMethod]
    public void CreateAttribute_ValidAttribute_ShouldCallAddAndReturnAttribute()
    {
        _attributeRepositoryMock!
            .Setup(repo => repo.Add(It.IsAny<ObjectSim.Domain.Attribute>()))
            .Returns(_attribute!);

        var result = _service!.Create(_attribute!);

        result.Should().NotBeNull();
        result.Should().Be(_attribute);

        _attributeRepositoryMock.Verify(repo => repo.Add(It.IsAny<ObjectSim.Domain.Attribute>()), Times.Once);
    }

    [TestMethod]
    public void CreateAttribute_NullAttribute_ShouldThrowInvalidOperationException()
    {
        Action act = () => _service!.Create(null!);

        act.Should().Throw<InvalidOperationException>()
           .WithMessage("Attribute cannot be null.");
    }

    [TestMethod]
    public void CreateAttribute_NullAttribute_ShouldNotCallRepository()
    {
        Action act = () => _service!.Create(null!);

        act.Should().Throw<InvalidOperationException>();
        _attributeRepositoryMock!.Verify(repo => repo.Add(It.IsAny<ObjectSim.Domain.Attribute>()), Times.Never);
    }
    [TestMethod]
    public void GetAllAttribute_CorrectAttrubtes_ShouldThrowAllOfThem()
    {
        var attributes = new List<Domain.Attribute>
        {
            new ObjectSim.Domain.Attribute
        {
            Id = Guid.NewGuid(),
            Name = "Nombre",
            ClassId = Guid.NewGuid(),
            DataType = Domain.ValueType.Create("int"),
            Visibility = Domain.Attribute.AttributeVisibility.Private
        },
            new ObjectSim.Domain.Attribute
        {
            Id = Guid.NewGuid(),
            Name = "Nombre2",
            ClassId = Guid.NewGuid(),
            DataType = Domain.ValueType.Create("int"),
            Visibility = Domain.Attribute.AttributeVisibility.Private
            }
        };

        Assert.IsNotNull(_attributeRepositoryMock);
        Assert.IsNotNull(_service);
        _attributeRepositoryMock.Setup(repo => repo.GetAll(It.IsAny<Func<Domain.Attribute, bool>>()))
            .Returns(attributes);
        var result = _service.GetAll();
        result.Should().HaveCount(2);
        _attributeRepositoryMock.Verify(repo => repo.GetAll(It.IsAny<Func<Domain.Attribute, bool>>()), Times.Once);
    }
    [TestMethod]
    public void GetAll_ShouldThrowException_WhenRepositoryReturnsEmptyList()
    {
        _attributeRepositoryMock!
            .Setup(repo => repo.GetAll(It.IsAny<Func<Domain.Attribute, bool>>()))
            .Returns([]);

        Action act = () => _service!.GetAll();

        act.Should().Throw<Exception>().WithMessage("No attributes found.");
        _attributeRepositoryMock.Verify(repo => repo.GetAll(It.IsAny<Func<Domain.Attribute, bool>>()), Times.Once);
    }
    [TestMethod]
    public void GetAll_ShouldThrowException_WhenRepositoryReturnsNull()
    {
        _attributeRepositoryMock!
            .Setup(repo => repo.GetAll(It.IsAny<Func<Domain.Attribute, bool>>()))
            .Returns(((List<Domain.Attribute>)(null!)));

        Action act = () => _service!.GetAll();

        act.Should().Throw<Exception>()
           .WithMessage("No attributes found.");

        _attributeRepositoryMock.Verify(repo => repo.GetAll(It.IsAny<Func<Domain.Attribute, bool>>()), Times.Once);
    }
    [TestMethod]
    public void DeleteAttribute_ValidId_ShouldCallDelete()
    {
        _attributeRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Domain.Attribute, bool>>()))
            .Returns(_attribute);

        _attributeRepositoryMock!
            .Setup(repo => repo.Delete(It.IsAny<Domain.Attribute>()));

        _service!.Delete(_attribute!.Id);

        _attributeRepositoryMock.Verify(repo => repo.Get(It.IsAny<Func<Domain.Attribute, bool>>()), Times.Once);
        _attributeRepositoryMock.Verify(repo => repo.Delete(It.IsAny<Domain.Attribute>()), Times.Once);
    }
    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void DeleteAttribute_InvalidId_ShouldThrowException()
    {
        _attributeRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Domain.Attribute, bool>>()))
            .Returns((Domain.Attribute)null!);

        _service!.Delete(Guid.Empty);
    }
    [TestMethod]
    public void GetById_ValidId_ShouldReturnAttribute()
    {
        _attributeRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Domain.Attribute, bool>>()))
            .Returns(_attribute);

        var result = _service!.GetById(_attribute!.Id);

        result.Should().NotBeNull();
        result.Should().Be(_attribute);

        _attributeRepositoryMock.Verify(repo => repo.Get(It.IsAny<Func<Domain.Attribute, bool>>()), Times.Once);
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void GetById_InvalidId_ShouldThrowArgumentException()
    {
        _service!.GetById(Guid.Empty);
    }
    [TestMethod]
    [ExpectedException(typeof(KeyNotFoundException))]
    public void GetById_NonExistentId_ShouldThrowKeyNotFoundException()
    {
        _attributeRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Domain.Attribute, bool>>()))
            .Returns((Domain.Attribute)null!);

        _service!.GetById(Guid.NewGuid());
    }
    [TestMethod]
    public void UpdateAttribute_ValidAttribute_ShouldUpdateAndReturnAttribute()
    {
        _attribute!.Name = "UpdatedName";
        _attributeRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Domain.Attribute, bool>>()))
            .Returns(_attribute);

        _attributeRepositoryMock!
            .Setup(repo => repo.Update(It.IsAny<Domain.Attribute>()))
            .Returns((Domain.Attribute attr) => attr);

        var result = _service!.Update(_attribute.Id, _attribute);

        result.Should().NotBeNull();
        result.Name.Should().Be("UpdatedName");

        _attributeRepositoryMock.Verify(repo => repo.Get(It.IsAny<Func<Domain.Attribute, bool>>()), Times.Once);
        _attributeRepositoryMock.Verify(repo => repo.Update(It.IsAny<Domain.Attribute>()), Times.Once);
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void UpdateAttribute_NullAttribute_ShouldThrowArgumentNullException()
    {
        _service!.Update(Guid.NewGuid(), null!);
    }
    [TestMethod]
    [ExpectedException(typeof(KeyNotFoundException))]
    public void UpdateAttribute_NonExistentAttribute_ShouldThrowKeyNotFoundException()
    {
        _attributeRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Domain.Attribute, bool>>()))
            .Returns((Domain.Attribute)null!);

        var dummyAttribute = new Domain.Attribute
        {
            Id = Guid.NewGuid(),
            Name = "Attr",
            ClassId = Guid.NewGuid(),
            Visibility = Domain.Attribute.AttributeVisibility.Public,
            DataType = Domain.ValueType.Create("int")
        };

        _service!.Update(dummyAttribute.Id, dummyAttribute);
    }
}
