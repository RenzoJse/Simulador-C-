using FluentAssertions;
using Moq;
using ObjectSim.BusinessLogic.ClassLogic.ClassBuilders;
using ObjectSim.BusinessLogic.ClassLogic.ClassBuilders.Builders;
using ObjectSim.Domain;
using ObjectSim.IBusinessLogic;
using Attribute = ObjectSim.Domain.Attribute;

namespace ObjectSim.BusinessLogic.Test.ClassLogic.ClassBuildersTest;

[TestClass]
public class BuilderTest
{
    private Builder? _builder;
    private Mock<IMethodService>? _methodServiceMock;
    private Mock<IClassService>? _classServiceMock;
    private Mock<IAttributeService>? _attributeServiceMock;

    private static readonly Guid ParentId = Guid.NewGuid();

    private static readonly Attribute TestAttribute = new Attribute
    {
        Id = Guid.NewGuid(),
        Name = "TestAttribute",
    };

    private readonly Class _parentClass = new Class
        {
            Id = ParentId,
            Name = "ParentClass",
            IsAbstract = false,
            IsSealed = false,
            IsInterface = false,
            Methods = [],
            Attributes = [TestAttribute],
            Parent = null,
        };

    [TestInitialize]
    public void Initialize()
    {
        _methodServiceMock = new Mock<IMethodService>(MockBehavior.Strict);
        _classServiceMock = new Mock<IClassService>(MockBehavior.Strict);
        _attributeServiceMock = new Mock<IAttributeService>(MockBehavior.Strict);
        _builder = new ClassBuilder(_methodServiceMock.Object, _classServiceMock.Object, _attributeServiceMock.Object);
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

        _classServiceMock!.Setup(m => m.GetById(parentId))
            .Returns(_parentClass);

        _attributeServiceMock!.Setup(m => m.GetById(existingAttributeId))
            .Returns(TestAttribute);

        _builder!.SetParent(parentId);

        Action action = () => _builder.SetAttributes([existingAttributeId]);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Attribute already exists in parent class");
    }

    [TestMethod]
    public void CreateClass_WithSameAttributesNameAsParent_ThrowsException()
    {
        var existingAttributeId = Guid.NewGuid();

        Attribute parentTestAttribute = new Attribute
        {
            Id = existingAttributeId,
            Name = "TestAttribute",
        };

        _parentClass.Attributes = [parentTestAttribute];

        var childAttributeId = Guid.NewGuid();

        Attribute childTestAttribute = new Attribute
        {
            Id = childAttributeId,
            Name = "TestAttribute",
        };

        _classServiceMock!.Setup(m => m.GetById(ParentId))
            .Returns(_parentClass);

        _attributeServiceMock!.Setup(m => m.GetById(childAttributeId))
            .Returns(childTestAttribute);

        _builder!.SetParent(ParentId);

        Action action = () => _builder.SetAttributes([childAttributeId]);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Attribute name already exists in parent class");
    }

    #endregion

    #region Success

    [TestMethod]
    public void CreateClass_WithEmptyAttributes_SetsEmptyAttributes()
    {
        _builder!.SetAttributes([]);

        _builder.GetResult().Attributes.Should().BeEmpty();
    }

    [TestMethod]
    public void CreateClass_WithValidAttributes_SetsAttributes()
    {
        _attributeServiceMock!.Setup(m => m.GetById(TestAttribute.Id))
            .Returns(TestAttribute);

        _builder!.SetAttributes([TestAttribute.Id]);

        _builder.GetResult().Attributes.Should().Contain(TestAttribute);
    }

    [TestMethod]
    public void CreateClass_WithParentWithoutAttributes_SetsAttributes()
    {
        _parentClass.Attributes = [];

        _classServiceMock!.Setup(m => m.GetById(ParentId))
            .Returns(_parentClass);

        _attributeServiceMock!.Setup(m => m.GetById(TestAttribute.Id))
            .Returns(TestAttribute);

        _builder!.SetParent(ParentId);
        _builder.SetAttributes([TestAttribute.Id]);

        _builder.GetResult().Attributes.Should().Contain(TestAttribute);
    }

    [TestMethod]
    public void CreateClass_WithMultipleValidAttributes_AddsAllAttributes()
    {
        var attribute1Id = Guid.NewGuid();
        var attribute2Id = Guid.NewGuid();

        var attribute1 = new Attribute { Id = attribute1Id, Name = "Attribute1" };
        var attribute2 = new Attribute { Id = attribute2Id, Name = "Attribute2" };

        _attributeServiceMock!.Setup(m => m.GetById(attribute1Id))
            .Returns(attribute1);
        _attributeServiceMock.Setup(m => m.GetById(attribute2Id))
            .Returns(attribute2);

        _builder!.SetAttributes([attribute1Id, attribute2Id]);

        _builder.GetResult().Attributes.Should().HaveCount(2);
        _builder.GetResult().Attributes.Should().Contain(attribute1);
        _builder.GetResult().Attributes.Should().Contain(attribute2);
    }

    [TestMethod]
    public void CreateClass_WithSameAndDifferentAttributeNameAsParent_SetsOnlyDifferentName()
    {
        var invalidAttributeId = Guid.NewGuid();
        var validAttributeId = Guid.NewGuid();

        Attribute invalidAttribute = new Attribute
        {
            Id = invalidAttributeId,
            Name = "TestAttribute"
        };

        Attribute validAttribute = new Attribute
        {
            Id = validAttributeId,
            Name = "NewAttribute"
        };

        _classServiceMock!.Setup(m => m.GetById(ParentId))
            .Returns(_parentClass);

        _attributeServiceMock!.Setup(m => m.GetById(invalidAttributeId))
            .Returns(invalidAttribute);

        _attributeServiceMock!.Setup(m => m.GetById(validAttributeId))
            .Returns(validAttribute);

        _builder!.SetParent(ParentId);

        Action invalidAction = () => _builder.SetAttributes([invalidAttributeId, validAttributeId]);
        invalidAction.Should().Throw<ArgumentException>()
            .WithMessage("Attribute name already exists in parent class");

        _builder.SetAttributes([validAttributeId]);
        _builder.GetResult().Attributes.Should().HaveCount(1);
        _builder.GetResult().Attributes.Should().Contain(validAttribute);
    }

    #endregion

    #endregion

    #region SetMethods

    #region Error

    [TestMethod]
    public void CreateClass_WithNullMethods_ThrowsException()
    {
        Action action = () => _builder!.SetMethods(null!);

        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void CreateClass_WithNotExistingMethods_ThrowsException()
    {
        var notExistingMethodId = Guid.NewGuid();

        _methodServiceMock!.Setup(m => m.GetById(notExistingMethodId))
            .Throws(new ArgumentException("Method does not exist"));

        Action action = () => _builder!.SetMethods([notExistingMethodId]);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Method does not exist");
    }

    [TestMethod]
    public void CreateClass_WithExactSameMethodsAsParent_ThrowsException()
    {
        var parentId = Guid.NewGuid();
        var existingMethodId = Guid.NewGuid();

        var parentMethod = new Method
        {
            Id = existingMethodId,
            Name = "TestMethod",
        };

        _parentClass.Methods = [parentMethod];

        var childMethodId = Guid.NewGuid();

        var childMethod = new Method
        {
            Id = childMethodId,
            Name = "TestMethod",
        };

        _classServiceMock!.Setup(m => m.GetById(parentId))
            .Returns(_parentClass);

        _methodServiceMock!.Setup(m => m.GetById(childMethodId))
            .Returns(childMethod);

        _builder!.SetParent(parentId);

        Action action = () => _builder.SetMethods([childMethodId]);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Method already exists in parent class");
    }

    #region Success

    [TestMethod]
    public void CreateClass_WithValidMethods_SetsMethods()
    {
        var methodId = Guid.NewGuid();
        var method = new Method
        {
            Id = methodId,
            Name = "TestMethod",
        };

        _methodServiceMock!.Setup(m => m.GetById(methodId))
            .Returns(method);

        _builder!.SetMethods([methodId]);

        _builder.GetResult().Methods.Should().Contain(method);
    }

    [TestMethod]
    public void CreateClass_ValidOverrideMethod_SetsMethods()
    {
        var parentId = Guid.NewGuid();
        var existingMethodId = Guid.NewGuid();

        var parentMethod = new Method
        {
            Id = existingMethodId,
            Name = "TestMethod",
        };

        _parentClass.Methods = [parentMethod];

        var childMethodId = Guid.NewGuid();

        var childMethod = new Method
        {
            Id = childMethodId,
            Name = "TestMethod",
        };

        _classServiceMock!.Setup(m => m.GetById(parentId))
            .Returns(_parentClass);

        _methodServiceMock!.Setup(m => m.GetById(childMethodId))
            .Returns(childMethod);

        _builder!.SetParent(parentId);
        _builder.SetMethods([childMethodId]);

        _builder.GetResult().Methods.Should().Contain(childMethod);
    }


    [TestMethod]
    public void CreateClass_WithSameMethodDefinitionAsParentPublicOne_SetsMethod()
    {
        var parentId = Guid.NewGuid();
        var existingMethodId = Guid.NewGuid();
        var parameter = new Parameter
        {
            Id = Guid.NewGuid(),
            Name = "TestParameter",
            Type = "int",
        };

        var parentMethod = new Method
        {
            Id = existingMethodId,
            Name = "TestMethod",
            Accessibility = "public",
            Parameters = [parameter],
        };

        _parentClass.Methods = [parentMethod];

        var childMethodId = Guid.NewGuid();

        var childMethod = new Method
        {
            Id = childMethodId,
            Name = "TestMethod",
            Accessibility = "public",
            Parameters = [parameter]
        };

        _classServiceMock!.Setup(m => m.GetById(parentId))
            .Returns(_parentClass);

        _methodServiceMock!.Setup(m => m.GetById(childMethodId))
            .Returns(childMethod);

        _builder!.SetParent(parentId);

        Action action = () => _builder.SetMethods([childMethodId]);

        _builder.GetResult().Methods.Should().Contain(childMethod);
    }

    [TestMethod]
    public void CreateClass_WithSameMethodDefinitionAsParentProtectedOne_SetsMethod()
    {
        var parentId = Guid.NewGuid();
        var existingMethodId = Guid.NewGuid();
        var parameter = new Parameter
        {
            Id = Guid.NewGuid(),
            Name = "TestParameter",
            Type = "int",
        };

        var parentMethod = new Method
        {
            Id = existingMethodId,
            Name = "TestMethod",
            Accessibility = "Protected",
            Parameters = [parameter],
        };

        _parentClass.Methods = [parentMethod];

        var childMethodId = Guid.NewGuid();

        var childMethod = new Method
        {
            Id = childMethodId,
            Name = "TestMethod",
            Accessibility = "Protected",
            Parameters = [parameter]
        };

        _classServiceMock!.Setup(m => m.GetById(parentId))
            .Returns(_parentClass);

        _methodServiceMock!.Setup(m => m.GetById(childMethodId))
            .Returns(childMethod);

        _builder!.SetParent(parentId);

        Action action = () => _builder.SetMethods([childMethodId]);

        _builder.GetResult().Methods.Should().Contain(childMethod);
    }

    [TestMethod]
    public void CreateClass_WithSameMethodDefinitionAsParentPrivateOne_SetsMethod()
    {
        var parentId = Guid.NewGuid();
        var existingMethodId = Guid.NewGuid();
        var parameter = new Parameter
        {
            Id = Guid.NewGuid(),
            Name = "TestParameter",
            Type = "int",
        };

        var parentMethod = new Method
        {
            Id = existingMethodId,
            Name = "TestMethod",
            Accessibility = "private",
            Parameters = [parameter],
        };

        _parentClass.Methods = [parentMethod];

        var childMethodId = Guid.NewGuid();

        var childMethod = new Method
        {
            Id = childMethodId,
            Name = "TestMethod",
            Accessibility = "private",
            Parameters = [parameter]
        };

        _classServiceMock!.Setup(m => m.GetById(parentId))
            .Returns(_parentClass);

        _methodServiceMock!.Setup(m => m.GetById(childMethodId))
            .Returns(childMethod);

        _builder!.SetParent(parentId);

        Action action = () => _builder.SetMethods([childMethodId]);

        _builder.GetResult().Methods.Should().Contain(childMethod);
    }

    [TestMethod]
    public void CreateClass_WithSameNameButDifferentDefinitionAsParentClass_SetsMethod()
    {
        var parentId = Guid.NewGuid();
        var existingMethodId = Guid.NewGuid();
        var parameter = new Parameter
        {
            Id = Guid.NewGuid(),
            Name = "TestParameter",
            Type = "int",
        };

        var parentMethod = new Method
        {
            Id = existingMethodId,
            Name = "TestMethod",
            Accessibility = "public",
            Parameters = [parameter],
        };

        _parentClass.Methods = [parentMethod];

        var childMethodId = Guid.NewGuid();

        var childMethod = new Method
        {
            Id = childMethodId,
            Name = "TestMethod",
            Accessibility = "private",
            Parameters = []
        };

        _classServiceMock!.Setup(m => m.GetById(parentId))
            .Returns(_parentClass);

        _methodServiceMock!.Setup(m => m.GetById(childMethodId))
            .Returns(childMethod);

        _builder!.SetParent(parentId);

        Action action = () => _builder.SetMethods([childMethodId]);

        _builder.GetResult().Methods.Should().Contain(childMethod);
    }

    #endregion

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
