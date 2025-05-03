using FluentAssertions;
using Moq;
using ObjectSim.ClassLogic.ClassBuilders.Builders;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;
using Attribute = ObjectSim.Domain.Attribute;

namespace ObjectSim.ClassLogic.Test.ClassBuildersTest;

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
        "int", "value");

    private static readonly CreateAttributeArgs TestCreateAttributeArgs = new(
        TestArgsDataType,
        "public",
        Guid.NewGuid(),
        "Test"
    );

    private static readonly CreateMethodArgs TestCreateMethodArgs = new(
        "TestMethod",
        new CreateDataTypeArgs("TestParameter", "int"),
        "public",
        false,
        false,
        false,
        Guid.NewGuid(),
        [],
        [],
        []
    );

    [TestInitialize]
    public void Initialize()
    {
        _methodServiceMock = new Mock<IMethodService>(MockBehavior.Strict);
        _classServiceMock = new Mock<IClassService>(MockBehavior.Strict);
        _attributeServiceMock = new Mock<IAttributeService>(MockBehavior.Strict);
        _classBuilderTest = new ClassBuilder(_methodServiceMock.Object, _attributeServiceMock.Object);
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

        _methodServiceMock!.Setup(m => m.CreateMethod(TestCreateMethodArgs)).Returns(TestMethod);

        Action action = () => _classBuilderTest!.SetMethods([TestCreateMethodArgs]);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Parent class is an interface. Should implement all his methods");
    }

    [TestMethod]
    public void SetMethods_WithMethodsThatCannotBeAdded_DoesNotSetInvalidMethods()
    {
        var invalidMethodArgs = new CreateMethodArgs(
            "InvalidMethod",
            new CreateDataTypeArgs("MethodType", "int"),
            "public",
            false,
            false,
            false,
            Guid.NewGuid(),
            [],
            [],
            []
        );

        var inValidMethod = new Method
        {
            Name = "InvalidMethod",
        };

        _methodServiceMock!.Setup(m => m.CreateMethod(TestCreateMethodArgs)).Returns(TestMethod);
        _methodServiceMock!.Setup(m => m.CreateMethod(invalidMethodArgs)).Returns(inValidMethod);

        _classBuilderTest!.SetMethods([TestCreateMethodArgs, invalidMethodArgs]);

        _classBuilderTest!.GetResult().Methods.Should().HaveCount(1);
        _classBuilderTest.GetResult().Methods.Should().Contain(TestMethod);
        _classBuilderTest.GetResult().Methods.Should().NotContain(inValidMethod);
    }

    #endregion

    #region Success

    [TestMethod]
    public void SetMethods_WithValidMethods_SetMethods()
    {
        var method1 = new Method { Name = "Method1" };

        var methodCreateArgs2 = new CreateMethodArgs(
            "Method2",
            new CreateDataTypeArgs("MethodType", "int"),
            "public",
            false,
            false,
            false,
            Guid.NewGuid(),
            [],
            [],
            []
        );

        var method2 = new Method { Name = "Method2" };

        _methodServiceMock!.Setup(m => m.CreateMethod(TestCreateMethodArgs)).Returns(method1);
        _methodServiceMock!.Setup(m => m.CreateMethod(methodCreateArgs2)).Returns(method2);

        _classBuilderTest!.SetMethods([TestCreateMethodArgs, methodCreateArgs2]);

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

        TestCreateMethodArgs.IsAbstract = true;

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

        _methodServiceMock!.Setup(m => m.CreateMethod(TestCreateMethodArgs)).Returns(interfaceMethod);

        _classBuilderTest!.SetMethods([TestCreateMethodArgs]);

        _classBuilderTest.GetResult().Methods.Should().HaveCount(1);
        _classBuilderTest.GetResult().Methods.Should().Contain(interfaceMethod);
    }

    #endregion

    #endregion

}
