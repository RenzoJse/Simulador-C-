using FluentAssertions;
using Moq;

namespace ObjectSim.BusinessLogic.Test;
[TestClass]
public class AttributeServiceTest
{
    private Mock<IAttributeRepository>? _attributeRepositoryMock;
    private AttributeService? _service;
    private Domain.ClassAttribute? _attribute;

    [TestInitialize]
    public void Setup()
    {
        _attributeRepositoryMock = new Mock<IAttributeRepository>(MockBehavior.Strict);
        _service = new AttributeService(_attributeRepositoryMock.Object);

        _attribute = new Domain.ClassAttribute
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

    [TestMethod]
    public void CreateAttribute_ValidAttribute_ShouldCallAddAndReturnAttribute()
    {
        _attributeRepositoryMock!
            .Setup(repo => repo.Add(It.IsAny<ClassAttribute>()))
            .Returns(_attribute!);

        var result = _service!.Create(_attribute!);

        result.Should().NotBeNull();
        result.Should().Be(_attribute);
        _attributeRepositoryMock.Verify(repo => repo.Add(It.IsAny<ClassAttribute>()), Times.Once);
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
        _attributeRepositoryMock!.Verify(repo => repo.Add(It.IsAny<ClassAttribute>()), Times.Never);
    }
}
