using FluentAssertions;
using Moq;
using ObjectSim.ClassConstructor.ClassBuilders.Builders;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;

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
        _attributeServiceMock = new Mock<IAttributeService>(MockBehavior.Strict);
        _methodServiceCreateMock = new Mock<IMethodServiceCreate>(MockBehavior.Strict);
        _abstractBuilder = new AbstractBuilder(_methodServiceCreateMock.Object, _attributeServiceMock.Object);
    }

    #region SetAttributes

    #region MyRegion

    [TestMethod]
    public void SetAttributes_ShouldNotThrow_WhenCalledWithEmptyList()
    {
        var attributes = new List<CreateAttributeArgs>();
        _abstractBuilder!.SetAttributes(attributes);
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
