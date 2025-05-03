using FluentAssertions;
using Moq;
using ObjectSim.ClassLogic.ClassBuilders;
using ObjectSim.ClassLogic.ClassBuilders.Builders;
using ObjectSim.Domain;
using ObjectSim.IBusinessLogic;
using Attribute = ObjectSim.Domain.Attribute;

namespace ObjectSim.ClassLogic.Test.ClassBuildersTest;

[TestClass]
public class BuilderTest
{
    private Builder? _builder;
    private Mock<IMethodServiceCreate>? _methodServiceCreateMock;
    private Mock<IClassService>? _classServiceMock;
    private Mock<IAttributeService>? _attributeServiceMock;

    private static readonly Guid ParentId = Guid.NewGuid();

    private static readonly Attribute TestAttribute = new Attribute
    {
        Id = Guid.NewGuid(),
        Name = "TestAttribute",
    };

    private readonly Class _parentClass = new()
    {
        Id = ParentId,
        Name = "ParentClass",
        IsAbstract = false,
        IsSealed = false,
        IsInterface = false,
        Methods = [],
        Attributes = [TestAttribute],
        Parent = null
    };

    [TestInitialize]
    public void Initialize()
    {
        _methodServiceCreateMock = new Mock<IMethodServiceCreate>(MockBehavior.Strict);
        _classServiceMock = new Mock<IClassService>(MockBehavior.Strict);
        _attributeServiceMock = new Mock<IAttributeService>(MockBehavior.Strict);
        _builder = new ClassBuilder(_methodServiceCreateMock.Object, _attributeServiceMock.Object);
    }

    #region SetParent

    #region Error

    [TestMethod]
    public void SetParent_SealedParent_ThrowsException()
    {
        var parentId = Guid.NewGuid();
        var sealedParent = new Class
        {
            Id = parentId,
            Name = "ParentClass",
            IsAbstract = false,
            IsSealed = true,
            IsInterface = false,
            Methods = [],
            Attributes = [],
            Parent = null,
        };

        _classServiceMock!.Setup(m => m.GetById(parentId))
            .Returns(sealedParent);

        Action action = () => _builder!.SetParent(sealedParent);

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

        _builder!.SetParent(parentClass);

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
    public void CreateClass_WithEmptyMethodsButParentIsInterface_ThrowsException()
    {
        var parentId = Guid.NewGuid();
        var interfaceMethod = new Method
        {
            Id = Guid.NewGuid(),
            Name = "InterfaceMethod",
            Accessibility = Method.MethodAccessibility.Public,
            Abstract = true,
        };

        var interfaceClass = new Class
        {
            Id = parentId,
            Name = "IParentInterface",
            IsAbstract = false,
            IsSealed = false,
            IsInterface = true,
            Methods = [interfaceMethod],
            Attributes = [],
            Parent = null
        };

        _classServiceMock!.Setup(m => m.GetById(parentId))
            .Returns(interfaceClass);

        _builder!.SetParent(interfaceClass);
        Action action = () => _builder.SetMethods([]);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Parent class is an interface and has methods that are not implemented");
    }

    #region Success

    [TestMethod]
    public void CreateClass_WithEmptyMethodList_SetsMethods()
    {
        _builder!.SetMethods([]);

        _builder.GetResult().Methods.Should().BeEmpty();
    }

    [TestMethod]
    public void CreateClass_WithEmptyMethodListWithParentNotBeingInterface_SetsEmptyMethods()
    {
        _classServiceMock!.Setup(m => m.GetById(_parentClass.Id))
            .Returns(_parentClass);

        _builder!.SetParent(_parentClass);

        _builder!.SetMethods([]);

        _builder.GetResult().Methods.Should().BeEmpty();
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
    public void CreateClass_WithValidAbstraction_SetsAbstraction()
    {
        _builder!.SetAbstraction(true);
        _builder.GetResult().IsAbstract.Should().BeTrue();
    }

    [TestMethod]
    public void CreateClass_WithValidInterface_SetsInterface()
    {
        _builder!.SetInterface(true);
        _builder.GetResult().IsInterface.Should().BeTrue();
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
        //TODO
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
