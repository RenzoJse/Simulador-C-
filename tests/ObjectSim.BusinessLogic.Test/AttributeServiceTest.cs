using FluentAssertions;
using Moq;
using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain;
using static ObjectSim.Domain.Attribute;

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
            Name = "Color",
            DataType = ReferenceType.Create("string"),
            Visibility = AttributeVisibility.Public
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
    public void GetAllTest()
    {
        var attributes = new List<Domain.Attribute>
        {
            new ObjectSim.Domain.Attribute
        {
            Id = Guid.NewGuid(),
            Name = "Nombre",
            ClassId = Guid.NewGuid(),
            DataType = Domain.ValueType.Create("int"),
            Visibility = ObjectSim.Domain.Attribute.AttributeVisibility.Private
        },
            new ObjectSim.Domain.Attribute
        {
            Id = Guid.NewGuid(),
            Name = "Nombre2",
            ClassId = Guid.NewGuid(),
            DataType = Domain.ValueType.Create("int"),
            Visibility = ObjectSim.Domain.Attribute.AttributeVisibility.Private
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
}
