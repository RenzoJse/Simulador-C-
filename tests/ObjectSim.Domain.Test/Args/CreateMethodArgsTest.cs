using FluentAssertions;
using ObjectSim.Domain.Args;

namespace ObjectSim.Domain.Test.Args;

[TestClass]
public class CreateMethodArgsTest
{
    [TestMethod]
    public void Name_SetAndGet_ShouldReturnExpectedValue()
    {
        var args = new CreateMethodArgs("Test", Guid.NewGuid(), "public", true, false, true, false, Guid.NewGuid(), [], [], []);
        const string newName = "Other";
        args.Name = newName;
        args.Name.Should().Be(newName);
    }

    [TestMethod]
    public void Type_SetAndGet_ShouldReturnExpectedValue()
    {
        var type = new CreateDataTypeArgs(Guid.NewGuid(), "int");
        var args = new CreateMethodArgs("Test", Guid.NewGuid(), "public", true, false, true, false, Guid.NewGuid(), [], [], []);
        var newType = Guid.NewGuid();
        args.TypeId = newType;
        args.TypeId.Should().Be(newType);
    }

    [TestMethod]
    public void Accessibility_SetAndGet_ShouldReturnExpectedValue()
    {
        var args = new CreateMethodArgs("Test", Guid.NewGuid(), "public", true, false, true, false, Guid.NewGuid(), [], [], []);
        const string newAccessibility = "private";
        args.Accessibility = newAccessibility;
        args.Accessibility.Should().Be(newAccessibility);
    }

    [TestMethod]
    public void IsAbstract_SetAndGet_ShouldReturnExpectedValue()
    {
        var args = new CreateMethodArgs("Test", Guid.NewGuid(), "public", true, false, true, false, Guid.NewGuid(), [], [], []);
        args.IsAbstract = false;
        args.IsAbstract.Should().BeFalse();
    }

    [TestMethod]
    public void IsSealed_SetAndGet_ShouldReturnExpectedValue()
    {
        var args = new CreateMethodArgs("Test", Guid.NewGuid(), "public", true, false, true, false, Guid.NewGuid(), [], [], [])
        {
            IsSealed = true
        };
        args.IsSealed.Should().BeTrue();
    }

    [TestMethod]
    public void IsOverride_SetAndGet_ShouldReturnExpectedValue()
    {
        var args = new CreateMethodArgs("Test", Guid.NewGuid(), "public", true, false, true, false, Guid.NewGuid(), [], [], [])
        {
            IsOverride = false
        };
        args.IsOverride.Should().BeFalse();
    }

    [TestMethod]
    public void ClassId_SetAndGet_ShouldReturnExpectedValue()
    {
        var args = new CreateMethodArgs("Test", Guid.NewGuid(), "public", true, false, true, false, Guid.NewGuid(), [], [], []);
        var newId = Guid.NewGuid();
        args.ClassId = newId;
        args.ClassId.Should().Be(newId);
    }

    [TestMethod]
    public void LocalVariables_SetAndGet_ShouldReturnExpectedValue()
    {
        var args = new CreateMethodArgs("Test", Guid.NewGuid(), "public", true, false, true, false, Guid.NewGuid(), [], [], []);
        var newVars = new List<CreateDataTypeArgs> { new(Guid.NewGuid(), "int") };
        args.LocalVariables = newVars;
        args.LocalVariables.Should().BeEquivalentTo(newVars);
    }

    [TestMethod]
    public void Parameters_SetAndGet_ShouldReturnExpectedValue()
    {
        var args = new CreateMethodArgs("Test", Guid.NewGuid(), "public", true, false, true, false, Guid.NewGuid(), [], [], []);
        var newParams = new List<CreateDataTypeArgs> { new(Guid.NewGuid(), "string") };
        args.Parameters = newParams;
        args.Parameters.Should().BeEquivalentTo(newParams);
    }

    [TestMethod]
    public void InvokeMethods_SetAndGet_ShouldReturnExpectedValue()
    {
        var args = new CreateMethodArgs("Test", Guid.NewGuid(), "public", true, false, true, false, Guid.NewGuid(), [], [], []);
        var newInvoke = new List<Guid> { Guid.NewGuid() };
        args.InvokeMethods = newInvoke;
        args.InvokeMethods.Should().BeEquivalentTo(newInvoke);
    }
}
