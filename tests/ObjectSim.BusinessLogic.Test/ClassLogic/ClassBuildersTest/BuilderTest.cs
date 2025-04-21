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

    [TestMethod]
    public void SetParent_SealedParent_ThrowsException()
    {
        var parentId = Guid.NewGuid();

        _classServiceMock!.Setup(m => m.GetById(parentId))
            .Returns(new Class
            {
                Id = parentId,
                Name = "ParentClass",
                IsAbstract = false,
                IsSealed = true,
                IsInterface = false,
                Methods = [],
                Attributes = [],
                Parent = null,
            });

        Action action = () => _builder!.SetParent(parentId);

        action.Should().Throw<ArgumentException>("Cant have a sealed class as parent");
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
        var validAttribute = new Attribute
        {
            Id = Guid.NewGuid(),
            Name = "ValidAttribute",
        };

        _builder!.SetAttributes([validAttribute]);

        _builder.GetResult().Attributes.Should().Contain(TestAttribute);
    }

    [TestMethod]
    public void CreateClass_WithMultipleValidAttributes_AddsAllAttributes()
    {
        var attribute1 = new Attribute { Name = "Attribute1" };
        var attribute2 = new Attribute { Name = "Attribute2" };

        _attributeServiceMock!.Setup(m => m.Create(attribute1))
            .Returns(attribute1);
        _attributeServiceMock.Setup(m => m.Create(attribute1))
            .Returns(attribute2);

        _builder!.SetAttributes([attribute1, attribute2]);

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

        _attributeServiceMock!.Setup(m => m.Create(invalidAttribute))
            .Throws(new ArgumentException("Attribute name already exists in parent class"));

        _attributeServiceMock!.Setup(m => m.GetById(validAttributeId))
            .Returns(validAttribute);

        _builder!.SetParent(ParentId);

        Action invalidAction = () => _builder.SetAttributes([invalidAttribute, validAttribute]);

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

    #region Success

    [TestMethod]
    public void CreateClass_WithValidMethods_SetsMethods()
    {
        var method = new Method
        {
            Name = "TestMethod",
        };

        _methodServiceMock!.Setup(m => m.Create(method))
            .Returns(method);

        _builder!.SetMethods([method]);

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

        _methodServiceMock!.Setup(m => m.Create(childMethod))
            .Returns(childMethod);

        _builder!.SetParent(parentId);
        _builder.SetMethods([childMethod]);

        _builder.GetResult().Methods.Should().Contain(childMethod);
    }

    [TestMethod]
    public void CreateClass_WithEmptyMethodsButParentIsInterface_SetsInterfaceMethods()
    {
        //TENGO QUE CAMBIARR PARA QUE DEVUELVA NO T IMPLEMENTED
        /*var parentId = Guid.NewGuid();
        var existingMethodId = Guid.NewGuid();

        var parentMethod = new Method
        {
            Id = existingMethodId,
            Name = "TestMethod",
            Accessibility = "public",
            Parameters = [],
        };

        _parentClass.Methods = [parentMethod];
        _parentClass.IsInterface = true;

        var childMethodId = Guid.NewGuid();

        var childMethod = new Method
        {
            Id = childMethodId,
            Name = "TestMethod",
            Accessibility = "public",
            Parameters = []
        };

        _classServiceMock!.Setup(m => m.GetById(parentId))
            .Returns(_parentClass);

        _methodServiceMock!.Setup(m => m.GetById(childMethodId))
            .Returns(childMethod);

        _builder!.SetParent(parentId);
        _builder.SetMethods([childMethodId]);

        _builder.GetResult().Methods.Should().Contain(childMethod);*/
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
        /*_builder!.SetName("ValidName");
        _builder.SetAbstraction(true);
        _builder.SetSealed(true);
        _builder.SetAttributes([Guid.NewGuid()]);
        _builder.SetMethods([Guid.NewGuid()]);
        _builder.SetParent(Guid.NewGuid());

        var result = _builder.GetResult();

        result.Should().NotBeNull();
        result.Name.Should().Be("ValidName");
        result.IsAbstract.Should().BeTrue();
        result.IsSealed.Should().BeTrue();*/
    }

    #endregion

}
