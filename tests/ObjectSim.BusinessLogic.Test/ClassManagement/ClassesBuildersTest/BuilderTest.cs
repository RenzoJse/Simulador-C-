using ClassManagement;
using ClassManagement.ClassesBuilders;
using ClassManagement.ClassesBuilders.Builders;
using FluentAssertions;
using ObjectSim.Domain;
using Attribute = ObjectSim.Domain.Attribute;

namespace ObjectSim.BusinessLogic.Test.ClassManagement.ClassesBuildersTest;

[TestClass]
public class BuilderTest
{
    private Builder? _builder;

    [TestInitialize]
    public void Initialize()
    {
        _builder = new ClassBuilder();
    }

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

    [TestMethod]
    public void CreateClass_WithNullAttributes_ThrowsException()
    {
        Action action = () => _builder!.SetAttributes(null!);

        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void CreateClass_WithNullMethods_ThrowsException()
    {
        Action action = () => _builder!.SetMethods(null!);

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
    public void CreateClass_WithValidAttributes_SetsAttributes()
    {
        var attributes = new List<Attribute>();

        _builder!.SetAttributes(attributes);
        _builder.GetResult().Attributes.Should().BeEquivalentTo(attributes);
    }

    [TestMethod]
    public void CreateClass_WithValidMethods_SetsMethods()
    {
        var methods = new List<Method>();

        _builder!.SetMethods(methods);
        _builder.GetResult().Methods.Should().BeEquivalentTo(methods);
    }

    [TestMethod]
    public void CreateClass_WithValidParent_SetsParent()
    {
        var parent = new Class();

        _builder!.SetParent(parent);
        _builder.GetResult().Parent.Should().Be(parent);
    }

    #endregion

}
