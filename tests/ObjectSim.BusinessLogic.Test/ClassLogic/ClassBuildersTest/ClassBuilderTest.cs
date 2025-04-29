using FluentAssertions;
using Moq;
using ObjectSim.BusinessLogic.ClassLogic.ClassBuilders.Builders;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;
using Attribute = ObjectSim.Domain.Attribute;

namespace ObjectSim.BusinessLogic.Test.ClassLogic.ClassBuildersTest;

[TestClass]
public class ClassBuilderTest
{
    private ClassBuilder? _classBuilderTest;
    private Mock<IMethodService>? _methodServiceMock;
    private Mock<IClassService>? _classServiceMock;
    private Mock<IAttributeService>? _attributeServiceMock;

    private static readonly Method TestMethod = new()
    {
        Name = "TestMethod",
    };

    private static readonly CreateDataTypeArgs TestArgsDataType = new(
        "int");

    private static readonly CreateAttributeArgs TestCreateAttributeArgs = new(
        TestArgsDataType,
        "public",
        Guid.NewGuid(),
        "Test"
    );

    [TestInitialize]
    public void Initialize()
    {
        _methodServiceMock = new Mock<IMethodService>(MockBehavior.Strict);
        _classServiceMock = new Mock<IClassService>(MockBehavior.Strict);
        _attributeServiceMock = new Mock<IAttributeService>(MockBehavior.Strict);
        _classBuilderTest = new ClassBuilder(_methodServiceMock.Object, _classServiceMock.Object, _attributeServiceMock.Object);
    }

    #region SetAttributes

    #region Error

    [TestMethod]
    public void SetAttributes_Null_ThrowsException()
    {
        Action action = () => _classBuilderTest!.SetAttributes(null!);

        action.Should().Throw<ArgumentNullException>();
    }

    #endregion

    #region Success

    [TestMethod]
    public void SetAttribute_WithSameAndDifferentAttributeNameAsParent_SetsOnlyDifferentNameAttribute()
    {
        var invalidAttributeId = Guid.NewGuid();
        var validAttributeId = Guid.NewGuid();

        var invalidAttribute = new Attribute
        {
            Id = invalidAttributeId,
            Name = "TestAttribute"
        };

        var validAttribute = new Attribute
        {
            Id = validAttributeId,
            Name = "NewAttribute"
        };

        var invalidAttributeArgs = new CreateAttributeArgs(null!, "public", Guid.NewGuid(), "TestAttribute");
        var validAttributeArgs = new CreateAttributeArgs(null!, "public", Guid.NewGuid(), "NewAttribute");

        _attributeServiceMock!.Setup(m => m.CreateAttribute(invalidAttributeArgs))
            .Returns(invalidAttribute);

        _attributeServiceMock!.Setup(m => m.CreateAttribute(validAttributeArgs))
            .Returns(validAttribute);

        _classServiceMock!.Setup(m => m.CanAddAttribute(It.IsAny<Class>(), invalidAttribute))
            .Returns(false);

        _classServiceMock!.Setup(m => m.CanAddAttribute(It.IsAny<Class>(), validAttribute))
            .Returns(true);

        _classBuilderTest!.SetAttributes([invalidAttributeArgs, validAttributeArgs]);

        _classBuilderTest.GetResult().Attributes.Should().HaveCount(1);
        _classBuilderTest.GetResult().Attributes.Should().Contain(validAttribute);
    }

    [TestMethod]
    public void CreateClass_WithMultipleValidAttributes_AddsAllAttributes()
    {
        var attribute1 = new Attribute { Name = "Attribute1" };
        var attribute2 = new Attribute { Name = "Attribute2" };

        _attributeServiceMock!.Setup(m => m.CreateAttribute(It.Is<CreateAttributeArgs>(args => args.Name == "Attribute1")))
            .Returns(attribute1);

        _attributeServiceMock!.Setup(m => m.CreateAttribute(It.Is<CreateAttributeArgs>(args => args.Name == "Attribute2")))
            .Returns(attribute2);

        _classServiceMock!.Setup(m => m.CanAddAttribute(It.IsAny<Class>(), attribute1))
            .Returns(true);

        _classServiceMock!.Setup(m => m.CanAddAttribute(It.IsAny<Class>(), attribute2))
            .Returns(true);

        var attributeArgs1 = new CreateAttributeArgs(TestCreateAttributeArgs.DataType, "public", Guid.NewGuid(), "Attribute1");
        var attributeArgs2 = new CreateAttributeArgs(TestCreateAttributeArgs.DataType, "public", Guid.NewGuid(), "Attribute2");

        _classBuilderTest!.SetAttributes([attributeArgs1, attributeArgs2]);

        _classBuilderTest.GetResult().Attributes.Should().HaveCount(2);
        _classBuilderTest.GetResult().Attributes.Should().Contain(attribute1);
        _classBuilderTest.GetResult().Attributes.Should().Contain(attribute2);
    }

    [TestMethod]
    public void CreateClass_WithInValidAttributes_SetsEmptyAttributes()
    {
        var attribute1 = new Attribute { Name = "Attribute1" };

        _attributeServiceMock!.Setup(m => m.CreateAttribute(TestCreateAttributeArgs))
            .Returns(attribute1);

        _classServiceMock!.Setup(m => m.CanAddAttribute(It.IsAny<Class>(), attribute1))
            .Returns(false);

        _classBuilderTest!.SetAttributes([TestCreateAttributeArgs]);

        _classBuilderTest.GetResult().Attributes.Should().BeEmpty();
    }

    #endregion

    #endregion

    #region SetMethods

    #region Error

    [TestMethod]
    public void SetMethods_Null_ThrowsException()
    {
        Action action = () => _classBuilderTest!.SetMethods(null!);

        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void SetMethods_WhenParentIsInterfaceAndMethodsAreNotImplemented_ThrowsException()
    {
        var interfaceMethod = new Method
        {
            Name = "Method",
            Abstract = true,
        };

        var parentClass = new Class
        {
            Name = "ParentInterface",
            IsAbstract = true,
            IsSealed = false,
            IsInterface = true,
            Methods = [interfaceMethod],
            Attributes = [],
            Parent = null,
        };

        _classServiceMock!.Setup(m => m.GetById(parentClass.Id))
            .Returns(parentClass);

        _classBuilderTest!.SetParent(parentClass.Id);

        _methodServiceMock!.Setup(m => m.CreateMethod(TestMethod)).Returns(TestMethod);

        _classServiceMock!.Setup(m => m.CanAddMethod(It.IsAny<Class>(), TestMethod)).Returns(true);

        Action action = () => _classBuilderTest!.SetMethods([TestMethod]);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Parent class is an interface. Should implement all his methods");
    }

    [TestMethod]
    public void SetMethods_WithMethodsThatCannotBeAdded_DoesNotSetInvalidMethods()
    {
        var validMethod = new Method { Name = "ValidMethod" };
        var invalidMethod = new Method { Name = "InvalidMethod" };

        _methodServiceMock!.Setup(m => m.CreateMethod(validMethod)).Returns(validMethod);
        _methodServiceMock!.Setup(m => m.CreateMethod(invalidMethod)).Returns(invalidMethod);
        _classServiceMock!.Setup(m => m.CanAddMethod(It.IsAny<Class>(), validMethod)).Returns(true);
        _classServiceMock!.Setup(m => m.CanAddMethod(It.IsAny<Class>(), invalidMethod)).Returns(false);

        _classBuilderTest!.SetMethods([validMethod, invalidMethod]);

        _classBuilderTest!.GetResult().Methods.Should().HaveCount(1);
        _classBuilderTest.GetResult().Methods.Should().Contain(validMethod);
        _classBuilderTest.GetResult().Methods.Should().NotContain(invalidMethod);
    }

    #endregion

    #region Success

    [TestMethod]
    public void SetMethods_WithValidMethods_SetMethods()
    {
        var method1 = new Method { Name = "Method1" };
        var method2 = new Method { Name = "Method2" };

        _methodServiceMock!.Setup(m => m.CreateMethod(method1)).Returns(method1);
        _methodServiceMock!.Setup(m => m.CreateMethod(method2)).Returns(method2);
        _classServiceMock!.Setup(m => m.CanAddMethod(It.IsAny<Class>(), method1)).Returns(true);
        _classServiceMock!.Setup(m => m.CanAddMethod(It.IsAny<Class>(), method2)).Returns(true);

        _classBuilderTest!.SetMethods([method1, method2]);

        _classBuilderTest!.GetResult().Methods.Should().HaveCount(2);
        _classBuilderTest.GetResult().Methods.Should().Contain(method1);
        _classBuilderTest.GetResult().Methods.Should().Contain(method2);
    }

    [TestMethod]
    public void SetMethods_WhenParenIsInterfaceAndHaveAllImplementedMethods_SetMethods()
    {
        var interfaceMethod = new Method
        {
            Name = "Method",
            Abstract = true,
        };

        var parentClass = new Class
        {
            Name = "ParentInterface",
            IsAbstract = true,
            IsSealed = false,
            IsInterface = true,
            Methods = [interfaceMethod],
            Attributes = [],
            Parent = null,
        };

        _classServiceMock!.Setup(m => m.GetById(parentClass.Id))
            .Returns(parentClass);

        _classBuilderTest!.SetParent(parentClass.Id);

        _methodServiceMock!.Setup(m => m.CreateMethod(interfaceMethod)).Returns(interfaceMethod);
        _classServiceMock!.Setup(m => m.CanAddMethod(It.IsAny<Class>(), interfaceMethod)).Returns(true);

        _classBuilderTest!.SetMethods([interfaceMethod]);

        _classBuilderTest.GetResult().Methods.Should().HaveCount(1);
        _classBuilderTest.GetResult().Methods.Should().Contain(interfaceMethod);
    }

    #endregion

    #endregion

}
