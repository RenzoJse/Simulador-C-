using FluentAssertions;
using ObjectSim.BusinessLogic.ClassLogic;
using ObjectSim.BusinessLogic.ClassLogic.ClassBuilders.Builders;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using Attribute = System.Attribute;

namespace ObjectSim.BusinessLogic.Test.ClassLogic;

[TestClass]
public class ClassDirectorTest
{
    private ClassDirector? _classDirector;
    private const string ValidName = "ClassName";
    private const bool IsAbstract = true;
    private const bool IsSealed = true;
    private static readonly List<Guid> Attributes = [Guid.NewGuid(), Guid.NewGuid()];
    private static readonly List<Guid> Methods = [Guid.NewGuid(), Guid.NewGuid()];


    [TestInitialize]
    public void Initialize()
    {
        var builder = new ClassBuilder();
        _classDirector = new ClassDirector(builder);
    }

    #region Error

    [TestMethod]
    public void ConstructClass_WithWrongName_ThrowsException()
    {
        const string invalidName = "a";
        var args = new CreateClassArgs(invalidName, IsAbstract, IsSealed, Attributes, Methods, Guid.NewGuid());

        Action action = () => _classDirector!.ConstructClass(args);

        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void ConstructClass_WithNullAbstraction_ThrowsException()
    {
        var args = new CreateClassArgs(ValidName, null, IsSealed, Attributes, Methods, Guid.NewGuid());

        Action action = () => _classDirector!.ConstructClass(args);

        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void ConstructClass_WithNullSealed_ThrowsException()
    {
        var args = new CreateClassArgs(ValidName, IsAbstract, null, Attributes, Methods, Guid.NewGuid());

        Action action = () => _classDirector!.ConstructClass(args);

        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void ConstructClass_WithNullAttributes_ThrowsException()
    {
        var args = new CreateClassArgs(ValidName, IsAbstract, IsSealed, null!, Methods, Guid.NewGuid());

        Action action = () => _classDirector!.ConstructClass(args);

        action.Should().Throw<ArgumentException>();
    }

    #endregion
}
