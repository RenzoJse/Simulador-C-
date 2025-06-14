using FluentAssertions;
using ObjectSim.Domain.Args;

namespace ObjectSim.Domain.Test.Args;

[TestClass]
public class CreateMethodArgsTest
{
    private CreateMethodArgs _args = null!;

    [TestInitialize]
    public void Setup()
    {
        _args = new CreateMethodArgs(
            "Test",
            Guid.NewGuid(),
            "public",
            true,
            false,
            true,
            false,
            false,
            Guid.NewGuid(),
            [],
            [],
            []
        );
    }

    #region Name

    [TestMethod]
    public void Name_SetAndGet_ShouldReturnExpectedValue()
    {
        const string newName = "Other";
        _args.Name = newName;
        _args.Name.Should().Be(newName);
    }

    #endregion

    #region Type

    [TestMethod]
    public void Type_SetAndGet_ShouldReturnExpectedValue()
    {
        var newType = Guid.NewGuid();
        _args.TypeId = newType;
        _args.TypeId.Should().Be(newType);
    }

    #endregion

    #region Accessibility

    [TestMethod]
    public void Accessibility_SetAndGet_ShouldReturnExpectedValue()
    {
        const string newAccessibility = "private";
        _args.Accessibility = newAccessibility;
        _args.Accessibility.Should().Be(newAccessibility);
    }

    #endregion

    #region IsAbstract

    [TestMethod]
    public void IsAbstract_SetAndGet_ShouldReturnExpectedValue()
    {
        _args.IsAbstract = false;
        _args.IsAbstract.Should().BeFalse();
    }

    #endregion

    #region IsSealed

    [TestMethod]
    public void IsSealed_SetAndGet_ShouldReturnExpectedValue()
    {
        _args.IsSealed = true;
        _args.IsSealed.Should().BeTrue();
    }

    #endregion

    #region IsOverride

    [TestMethod]
    public void IsOverride_SetAndGet_ShouldReturnExpectedValue()
    {
        _args.IsOverride = false;
        _args.IsOverride.Should().BeFalse();
    }

    #endregion

    #region IsStatic

    [TestMethod]
    public void IsStatic_SetAndGet_ShouldReturnExpectedValue()
    {
        _args.IsStatic = true;
        _args.IsStatic.Should().BeTrue();
    }

    #endregion

    #region ClassId

    [TestMethod]
    public void ClassId_SetAndGet_ShouldReturnExpectedValue()
    {
        var newId = Guid.NewGuid();
        _args.ClassId = newId;
        _args.ClassId.Should().Be(newId);
    }

    #endregion

    #region LocalVariables

    [TestMethod]
    public void LocalVariables_SetAndGet_ShouldReturnExpectedValue()
    {
        var newVars = new List<CreateVariableArgs> { new(Guid.NewGuid(), "int") };
        _args.LocalVariables = newVars;
        _args.LocalVariables.Should().BeEquivalentTo(newVars);
    }

    #endregion

    #region Parameters

    [TestMethod]
    public void Parameters_SetAndGet_ShouldReturnExpectedValue()
    {
        var newParams = new List<CreateVariableArgs> { new(Guid.NewGuid(), "string") };
        _args.Parameters = newParams;
        _args.Parameters.Should().BeEquivalentTo(newParams);
    }

    #endregion

    #region InvokeMethods

    [TestMethod]
    public void InvokeMethods_SetAndGet_ShouldReturnExpectedValue()
    {
        var newInvoke = new List<Guid> { Guid.NewGuid() };
        _args.InvokeMethods = newInvoke;
        _args.InvokeMethods.Should().BeEquivalentTo(newInvoke);
    }

    #endregion
}
