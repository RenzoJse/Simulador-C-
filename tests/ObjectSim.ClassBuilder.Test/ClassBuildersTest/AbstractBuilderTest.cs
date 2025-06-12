using FluentAssertions;
using Moq;
using ObjectSim.ClassConstructor.ClassBuilders.Builders;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;
using Attribute = ObjectSim.Domain.Attribute;
using ValueType = ObjectSim.Domain.ValueType;

namespace ObjectSim.ClassLogic.Test.ClassBuildersTest;

[TestClass]
public class AbstractBuilderTest
{
    private AbstractBuilder? _abstractBuilder;
    private Mock<IMethodServiceCreate>? _methodServiceCreateMock;
    private Mock<IAttributeService>? _attributeServiceMock;

    private static readonly Method TestMethod = new()
    {
        Name = "TestMethod",
    };

    private static readonly DataType TestDataType = new ValueType(Guid.NewGuid(), "int");

    private static readonly CreateAttributeArgs TestCreateAttributeArgs = new(
        Guid.NewGuid(),
        "public",
        Guid.NewGuid(),
        "Test"
    );

    private static readonly Class ParentClass = new Class
    {
        Id = Guid.NewGuid(),
        Name = "ParentClass",
        IsAbstract = false,
        IsSealed = false,
        IsInterface = false,
        Methods = [],
        Attributes = []
    };

    private static readonly CreateMethodArgs TestCreateMethodArgs = new(
        "TestMethod",
        Guid.NewGuid(),
        "public",
        false,
        false,
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
        _attributeServiceMock = new Mock<IAttributeService>(MockBehavior.Strict);
        _methodServiceCreateMock = new Mock<IMethodServiceCreate>(MockBehavior.Strict);
        _abstractBuilder = new AbstractBuilder(_methodServiceCreateMock.Object, _attributeServiceMock.Object);
    }

    #region SetAttributes

    #region Error

    [TestMethod]
    public void SetAttributes_Null_ThrowsException()
    {
        Action action = () => _abstractBuilder!.SetAttributes(null!);

        action.Should().Throw<ArgumentNullException>();
    }

    #endregion

    #region Success

    [TestMethod]
    public void SetAttribute_WithSameAndDifferentAttributeNameAsParent_SetsOnlyDifferentNameAttribute()
    {
        ParentClass.Attributes = [];

        var validAttributeId = Guid.NewGuid();

        var validAttribute = new Attribute
        {
            Id = validAttributeId,
            Name = "NewAttribute"
        };

        var parentAttribute = new Attribute
        {
            Id = Guid.NewGuid(),
            Name = "TestAttribute"
        };

        ParentClass.Attributes.Add(parentAttribute);

        _abstractBuilder!.SetParent(ParentClass);

        var invalidAttributeArgs = new CreateAttributeArgs(Guid.NewGuid(), "public", Guid.NewGuid(), "TestAttribute");
        var validAttributeArgs = new CreateAttributeArgs(Guid.NewGuid(), "public", Guid.NewGuid(), "NewAttribute");

        _attributeServiceMock!.Setup(m => m.CreateAttribute(invalidAttributeArgs))
            .Throws(new ArgumentException());

        _attributeServiceMock!.Setup(m => m.CreateAttribute(validAttributeArgs))
            .Returns(validAttribute);

        _abstractBuilder!.SetAttributes([invalidAttributeArgs, validAttributeArgs]);

        _abstractBuilder.GetResult().Attributes.Should().HaveCount(1);
        _abstractBuilder.GetResult().Attributes.Should().Contain(validAttribute);
    }

    [TestMethod]
    public void CreateClass_WithMultipleValidAttributes_AddsAllAttributes()
    {
        var attribute1 = new Attribute { Name = "Attribute1", DataType = TestDataType };
        var attribute2 = new Attribute { Name = "Attribute2", DataType = TestDataType };

        _attributeServiceMock!.Setup(m => m.CreateAttribute(It.Is<CreateAttributeArgs>(args => args.Name == "Attribute1")))
            .Returns(attribute1);

        _attributeServiceMock!.Setup(m => m.CreateAttribute(It.Is<CreateAttributeArgs>(args => args.Name == "Attribute2")))
            .Returns(attribute2);

        var attributeArgs1 = new CreateAttributeArgs(TestCreateAttributeArgs.DataTypeId, "public", Guid.NewGuid(), "Attribute1");
        var attributeArgs2 = new CreateAttributeArgs(TestCreateAttributeArgs.DataTypeId, "public", Guid.NewGuid(), "Attribute2");

        _abstractBuilder!.SetAttributes([attributeArgs1, attributeArgs2]);

        _abstractBuilder.GetResult().Attributes.Should().HaveCount(2);
        _abstractBuilder.GetResult().Attributes.Should().Contain(attribute1);
        _abstractBuilder.GetResult().Attributes.Should().Contain(attribute2);
    }

    [TestMethod]
    public void CreateClass_WithInValidAttributes_SetsEmptyAttributes()
    {
        _attributeServiceMock!.Setup(m => m.CreateAttribute(TestCreateAttributeArgs))
            .Throws(new ArgumentException());

        _abstractBuilder!.SetAttributes([TestCreateAttributeArgs]);

        _abstractBuilder.GetResult().Attributes.Should().BeEmpty();
    }

    #endregion

    #endregion

    #region SetMethods

    #region Error

    [TestMethod]
    public void SetMethods_Null_ThrowsException()
    {
        Action action = () => _abstractBuilder!.SetMethods(null!);

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

        _abstractBuilder!.SetParent(parentClass);

        _methodServiceCreateMock!.Setup(m => m.CreateMethod(TestCreateMethodArgs)).Returns(TestMethod);

        Action action = () => _abstractBuilder!.SetMethods([TestCreateMethodArgs]);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Parent class is an interface. Should implement all his methods");
    }

    [TestMethod]
    public void SetMethods_WithMethodsThatCannotBeAdded_DoesNotSetInvalidMethods()
    {
        var invalidMethodArgs = new CreateMethodArgs(
            "InvalidMethod",
            Guid.NewGuid(),
            "public",
            false,
            false,
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

        _methodServiceCreateMock!.Setup(m => m.CreateMethod(TestCreateMethodArgs)).Returns(TestMethod);
        _methodServiceCreateMock!.Setup(m => m.CreateMethod(invalidMethodArgs)).Throws(new ArgumentException());

        _abstractBuilder!.SetMethods([TestCreateMethodArgs, invalidMethodArgs]);

        _abstractBuilder!.GetResult().Methods.Should().HaveCount(1);
        _abstractBuilder.GetResult().Methods.Should().Contain(TestMethod);
        _abstractBuilder.GetResult().Methods.Should().NotContain(inValidMethod);
    }

    #endregion

    #region Success

    [TestMethod]
    public void SetMethods_WithValidMethods_SetMethods()
    {
        var method1 = new Method { Name = "Method1" };

        var methodCreateArgs2 = new CreateMethodArgs(
            "Method2",
            Guid.NewGuid(),
            "public",
            false,
            false,
            false,
            false,
            false,
            Guid.NewGuid(),
            [],
            [],
            []
        );

        var method2 = new Method { Name = "Method2" };

        _methodServiceCreateMock!.Setup(m => m.CreateMethod(TestCreateMethodArgs)).Returns(method1);
        _methodServiceCreateMock!.Setup(m => m.CreateMethod(methodCreateArgs2)).Returns(method2);

        _abstractBuilder!.SetMethods([TestCreateMethodArgs, methodCreateArgs2]);

        _abstractBuilder!.GetResult().Methods.Should().HaveCount(2);
        _abstractBuilder.GetResult().Methods.Should().Contain(method1);
        _abstractBuilder.GetResult().Methods.Should().Contain(method2);
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

        _abstractBuilder!.SetParent(parentClass);

        _methodServiceCreateMock!.Setup(m => m.CreateMethod(TestCreateMethodArgs)).Returns(interfaceMethod);

        _abstractBuilder!.SetMethods([TestCreateMethodArgs]);

        _abstractBuilder.GetResult().Methods.Should().HaveCount(1);
        _abstractBuilder.GetResult().Methods.Should().Contain(interfaceMethod);
    }

    #endregion

    #endregion

}
