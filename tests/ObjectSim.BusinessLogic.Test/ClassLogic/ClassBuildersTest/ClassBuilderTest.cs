using FluentAssertions;
using ObjectSim.BusinessLogic.ClassLogic.ClassBuilders.Builders;
using ObjectSim.Domain;

namespace ObjectSim.BusinessLogic.Test.ClassLogic.ClassBuildersTest;

[TestClass]
public class ClassBuilderTest
{
    private ClassBuilder? _classBuilderTest;

    [TestInitialize]
    public void Initialize()
    {
        _classBuilderTest = new ClassBuilder();
    }

    #region Error

    [TestMethod]
    public void SetAttributes_Null_ThrowsException()
    {
        Action action = () => _classBuilderTest!.SetAttributes(null!);

        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void SetMethods_Null_ThrowsException()
    {
        Action action = () => _classBuilderTest!.SetMethods(null!);

        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void SetMethods_WhenParentIsInterface_AndMethodsAreNotImplemented_ThrowsException()
    {
        var notImplementedMethod = new Method
        {
            Name = "NotImplementedMethod",
        };

        var parentClass = new Class
        {
            Name = "ParentInterfaceClass",
            IsAbstract = true,
            IsSealed = false,
            Methods = [notImplementedMethod],
            Attributes = [],
            Parent = null,
        };

        var childClass = new Class
        {
            Name = "ChildClass",
            IsAbstract = false,
            IsSealed = false,
            Methods = [],
            Attributes = [],
            Parent = parentClass,
        };

        Action action = () => _classBuilderTest!.SetMethods([Guid.NewGuid()]);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Parent class is an interface and has methods that are not implemented");
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

}
