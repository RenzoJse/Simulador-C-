using FluentAssertions;
using Moq;
using ObjectSim.BusinessLogic.ClassLogic.ClassBuilders.Builders;
using ObjectSim.Domain;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.BusinessLogic.Test.ClassLogic.ClassBuildersTest;

[TestClass]
public class ClassBuilderTest
{
    private ClassBuilder? _classBuilderTest;
    private Mock<IMethodService>? _methodServiceMock;
    private Mock<IClassService>? _classServiceMock;

    [TestInitialize]
    public void Initialize()
    {
        _methodServiceMock = new Mock<IMethodService>(MockBehavior.Strict);
        _classServiceMock = new Mock<IClassService>(MockBehavior.Strict);
        _classBuilderTest = new ClassBuilder(_methodServiceMock.Object, _classServiceMock.Object);
    }

    #region Error

    [TestMethod]
    public void SetAttributes_Null_ThrowsException()
    {
        Action action = () => _classBuilderTest!.SetAttributes(null!);

        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void SetMethods_WhenParentHaveAbstractMethods_AndMethodsAreNotImplemented_ThrowsException()
    {
    }

    [TestMethod]
    public void SetAttributes_WhenParentHavePublicAttributes_AndNewClassHaveSameAttributeNames_ThrowsException()
    {
    }

    #endregion

    [TestMethod]
    public void SetMethods_WhenParentIsInterface_AndMethodsAreImplemented_AddsMethods()
    {
    }

    [TestMethod]
    public void SetMethods_WhenParentHaveAbstractMethods_AndMethodsAreImplemented_AddsMethods()
    {
    }

    #region SetParent

    #region Error

    [TestMethod]
    public void SetParent_Null_ThrowsException()
    {
        Action action = () => _classBuilderTest!.SetParent(Guid.Empty);

        action.Should().Throw<ArgumentNullException>();
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

        _classBuilderTest!.SetParent(parentId);

        _classBuilderTest.GetResult().Parent.Should().Be(parentClass);
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
    public void SetMethods_WhenTryingToAddNonExistantMethodID_ThrowsException()
    {
        Guid nonExistentMethodId = Guid.NewGuid();

        _methodServiceMock!.Setup(m => m.GetById(nonExistentMethodId))
            .Returns((Method)null!);

        Action action = () => _classBuilderTest!.SetMethods([nonExistentMethodId]);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Method does not exist");
    }

    [TestMethod]
    public void SetMethods_WhenParentIsInterfaceAndMethodsAreNotImplemented_ThrowsException()
    {
        var methodId = Guid.NewGuid();

        var notImplementedMethod = new Method
        {
            Name = "NotImplementedMethod",
            Abstract = true,
            Id = methodId
        };

        var parentClass = new Class
        {
            Name = "ParentInterface",
            IsAbstract = true,
            IsSealed = false,
            IsInterface = true,
            Methods = [notImplementedMethod],
            Attributes = [],
            Parent = null,
        };

        _methodServiceMock!.Setup(m => m.GetById(methodId))
            .Returns(notImplementedMethod);

        _classBuilderTest!.SetParent(parentClass.Id);
        Action action = () => _classBuilderTest!.SetMethods([methodId]);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Parent class is an interface and has methods that are not implemented");
    }

    #endregion

    #region Success

    #endregion

    #endregion

}
