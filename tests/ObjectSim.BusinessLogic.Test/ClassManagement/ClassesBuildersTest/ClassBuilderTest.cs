using ClassManagement.ClassesBuilders.Builders;
using FluentAssertions;
using ObjectSim.Domain;
using Attribute = System.Attribute;

namespace ObjectSim.BusinessLogic.Test.ClassManagement.ClassesBuildersTest;

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
