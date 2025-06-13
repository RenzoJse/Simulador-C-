using FluentAssertions;
using ObjectSim.ClassConstructor.ClassBuilders.Builders;
using ObjectSim.Domain.Args;

namespace ObjectSim.ClassLogic.Test.ClassBuildersTest;

[TestClass]
public class InterfaceBuilderTest
{
    private InterfaceBuilder? _interfaceBuilderTest;

    private static readonly Guid TestAttributeId = Guid.NewGuid();

    private readonly CreateAttributeArgs _testArgsAttribute = new(
        Guid.NewGuid(),
        "public",
        TestAttributeId,
        "Test",
        false
    );

    private readonly CreateMethodArgs _testCreateMethodArgs = new CreateMethodArgs(
        "TestMethod",
        Guid.NewGuid(),
        "public",
        false,
        false,
        false,
        false,
        false,
        Guid.Empty,
        [],
        [],
        []
    );

    [TestInitialize]
    public void Initialize()
    {
        _interfaceBuilderTest = new InterfaceBuilder();
    }

    #region SetAttributes

    #region Error

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void SetAttributes_NullAttributeList_ThrowsException()
    {
        _interfaceBuilderTest!.SetAttributes(null!);
    }

    #endregion

    #region Success

    [TestMethod]
    public void SetAttributes_WithAttributeList_AddsEmptyAttributeList()
    {
        _interfaceBuilderTest!.SetAttributes([_testArgsAttribute]);
        var result = _interfaceBuilderTest.GetResult();

        result.Attributes.Should().BeEmpty();
    }

    #endregion

    #endregion

    #region SetMethods

    #region Error

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void SetMethods_NullMethodList_ThrowsException()
    {
        _interfaceBuilderTest!.SetMethods(null!);
    }

    #endregion

    #region Success

    [TestMethod]
    public void SetMethods_WithMethodList_AddsEmptyMethodList()
    {
        _interfaceBuilderTest!.SetMethods([_testCreateMethodArgs]);
        var result = _interfaceBuilderTest.GetResult();

        result.Methods.Should().BeEmpty();
    }

    #endregion

    #endregion

    #region SetAbstraction

    [TestMethod]
    public void SetAbstraction_WithAbstractionValue_SetsAbstraction()
    {
        _interfaceBuilderTest!.SetAbstraction(false);
        var result = _interfaceBuilderTest.GetResult();

        result.IsAbstract.Should().BeTrue();
    }

    #endregion

}
