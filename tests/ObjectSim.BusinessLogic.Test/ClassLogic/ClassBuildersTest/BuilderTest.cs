using FluentAssertions;
using Moq;
using ObjectSim.BusinessLogic.ClassLogic.ClassBuilders;
using ObjectSim.BusinessLogic.ClassLogic.ClassBuilders.Builders;
using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain;
using ObjectSim.IBusinessLogic;
using Attribute = ObjectSim.Domain.Attribute;

namespace ObjectSim.BusinessLogic.Test.ClassLogic.ClassBuildersTest;

[TestClass]
public class BuilderTest
{
    private Builder? _builder;
    private Mock<IRepository<Method>>? _methodRepositoryMock;
    private Mock<IMethodService>? _methodServiceMock;
    private Mock<IClassService>? _classServiceMock;
    private Mock<IAttributeService>? _attributeServiceMock;

    [TestInitialize]
    public void Initialize()
    {
        _methodServiceMock = new Mock<IMethodService>(MockBehavior.Strict);
        _classServiceMock = new Mock<IClassService>(MockBehavior.Strict);
        _attributeServiceMock = new Mock<IAttributeService>(MockBehavior.Strict);
        _builder = new ClassBuilder(_methodServiceMock.Object, _classServiceMock.Object, _attributeServiceMock.Object);
        _methodRepositoryMock = new Mock<IRepository<Method>>(MockBehavior.Strict);
    }

    #region SetParent

    #region Error

    [TestMethod]
    public void SetParent_InvalidParentID_ThrowsException()
    {
        var invalidParentId = Guid.NewGuid();

        _classServiceMock!.Setup(m => m.GetById(invalidParentId))
            .Throws(new ArgumentException("Class does not exist"));

        Action action = () => _builder!.SetParent(invalidParentId);

        action.Should().Throw<ArgumentException>("Class does not exist");
    }

    #endregion

    #region Success

    [TestMethod]
    public void SetParent_ValidParentID_AddsParent()
    {
        var parentId = Guid.NewGuid();
        var parentClass = new Class
        {
            Id = parentId,
            Name = "ParentClass",
            IsAbstract = false,
            IsSealed = false,
            IsInterface = false,
            Methods = [],
            Attributes = [],
            Parent = null,
        };

        _classServiceMock!.Setup(m => m.GetById(parentId))
            .Returns(parentClass);

        _builder!.SetParent(parentId);

        _builder.GetResult().Parent.Should().Be(parentClass);
    }

    #endregion

    #endregion

    #region SetAttributes

    #region Error

    [TestMethod]
    public void CreateClass_WithNullAttributes_ThrowsException()
    {
        Action action = () => _builder!.SetAttributes(null!);

        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void CreateClass_WithNotExistingAttributes_ThrowsException()
    {
        var notExistingAttributeId = Guid.NewGuid();

        _attributeServiceMock!.Setup(m => m.GetById(notExistingAttributeId))
            .Throws(new ArgumentException("Attribute does not exist"));

        Action action = () => _builder!.SetAttributes([notExistingAttributeId]);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Attribute does not exist");
    }

    [TestMethod]
    public void CreateClass_WithSameAttributesAsParent_ThrowsException()
    {
        var parentId = Guid.NewGuid();
        var existingAttributeId = Guid.NewGuid();
        Attribute testAttribute = new Attribute
        {
            Id = existingAttributeId,
            Name = "TestAttribute",
        };

        var parentClass = new Class
        {
            Id = parentId,
            Name = "ParentClass",
            IsAbstract = false,
            IsSealed = false,
            IsInterface = false,
            Methods = [],
            Attributes = [testAttribute],
            Parent = null,
        };

        _classServiceMock!.Setup(m => m.GetById(parentId))
            .Returns(parentClass);

        _attributeServiceMock!.Setup(m => m.GetById(existingAttributeId))
            .Returns(testAttribute);

        _builder!.SetParent(parentId);

        Action action = () => _builder.SetAttributes([existingAttributeId]);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Attribute already exists in parent class");
    }

    [TestMethod]
    public void CreateClass_WithSameAttributesNameAsParent_ThrowsException()
    {
        var parentId = Guid.NewGuid();
        var existingAttributeId = Guid.NewGuid();
        Attribute parentTestAttribute = new Attribute
        {
            Id = existingAttributeId,
            Name = "TestAttribute",
        };

        var childAttributeId = Guid.NewGuid();

        Attribute childTestAttribute = new Attribute
        {
            Id = childAttributeId,
            Name = "TestAttribute",
        };

        var parentClass = new Class
        {
            Id = parentId,
            Name = "ParentClass",
            IsAbstract = false,
            IsSealed = false,
            IsInterface = false,
            Methods = [],
            Attributes = [parentTestAttribute],
            Parent = null,
        };

        _classServiceMock!.Setup(m => m.GetById(parentId))
            .Returns(parentClass);

        _attributeServiceMock!.Setup(m => m.GetById(childAttributeId))
            .Returns(childTestAttribute);

        _builder!.SetParent(parentId);

        Action action = () => _builder.SetAttributes([childAttributeId]);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Attribute name already exists in parent class");
    }

    #endregion

    #region Success

    [TestMethod]
    public void CreateClass_WithValidAttributes_SetsAttributes()
    {
        var attributeId = Guid.NewGuid();
        var testAttribute = new Attribute
        {
            Id = attributeId,
            Name = "TestAttribute",
        };

        _attributeServiceMock!.Setup(m => m.GetById(attributeId))
            .Returns(testAttribute);

        _builder!.SetAttributes([attributeId]);

        _builder.GetResult().Attributes.Should().Contain(testAttribute);
    }

    #endregion

    #endregion

    #region Error

    [TestMethod]
    public void CreateClass_WithNullName_ThrowsException()
    {
        Action action = () => _builder!.SetName(null!);

        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void CreateClass_WithNullAbstraction_ThrowsException()
    {
        Action action = () => _builder!.SetAbstraction(null!);

        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void CreateClass_WithNullSealed_ThrowsException()
    {
        Action action = () => _builder!.SetSealed(null!);

        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void CreateClass_WithNullMethods_ThrowsException()
    {
        Action action = () => _builder!.SetMethods(null!);

        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void CreateClass_WithNotExistingMethods_ThrowsException()
    {
    }

    #endregion

    #region Success

    [TestMethod]
    public void CreateClass_WithValidName_SetsName()
    {
        _builder!.SetName("ValidName");
        _builder.GetResult().Name.Should().Be("ValidName");
    }

    [TestMethod]
    public void CreateClass_WithNullAbstraction_SetsAbstraction()
    {
        _builder!.SetAbstraction(true);
        _builder.GetResult().IsAbstract.Should().BeTrue();
    }

    [TestMethod]
    public void CreateClass_WithValidSealed_SetsSealed()
    {
        _builder!.SetSealed(true);
        _builder.GetResult().IsSealed.Should().BeTrue();
    }

    [TestMethod]
    public void CreateClass_WithValidMethods_SetsMethods()
    {
    }

    [TestMethod]
    public void CreateClass_GetResultWhenCreationIsValid_ReturnsClass()
    {
        _builder!.SetName("ValidName");
        _builder.SetAbstraction(true);
        _builder.SetSealed(true);
        _builder.SetAttributes([Guid.NewGuid()]);
        _builder.SetMethods([Guid.NewGuid()]);
        _builder.SetParent(Guid.NewGuid());

        var result = _builder.GetResult();

        result.Should().NotBeNull();
        result.Name.Should().Be("ValidName");
        result.IsAbstract.Should().BeTrue();
        result.IsSealed.Should().BeTrue();
    }

    #endregion

}
