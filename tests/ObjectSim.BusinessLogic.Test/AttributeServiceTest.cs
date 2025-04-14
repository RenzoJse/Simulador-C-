using FluentAssertions;
using Moq;
using ObjectSim.IDataAccess;
using static ObjectSim.Domain.Attribute;

namespace ObjectSim.BusinessLogic.Test;
[TestClass]
public class AttributeServiceTest
{
    private Mock<IAttributeRepository>? _attributeRepositoryMock;
    private AttributeService? _service;
    private ObjectSim.Domain.Attribute? _attribute;

    [TestInitialize]
    public void Setup()
    {
        _attributeRepositoryMock = new Mock<IAttributeRepository>(MockBehavior.Strict);
        _service = new AttributeService(_attributeRepositoryMock.Object);

        _attribute = new ObjectSim.Domain.Attribute
        {
            Id = 1,
            Name = "Color",
            DataType = AttributeDataType.String,
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
}
