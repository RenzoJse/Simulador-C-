using FluentAssertions;
using ObjectSim.BusinessLogic.ClassLogic.ClassBuilders.Builders;
using Attribute = ObjectSim.Domain.Attribute;

namespace ObjectSim.BusinessLogic.Test.ClassLogic.ClassBuildersTest;

[TestClass]
public class InterfaceBuilderTest
{
    private InterfaceBuilder? _interfaceBuilderTest;

    [TestInitialize]
    public void Initialize()
    {
        _interfaceBuilderTest = new InterfaceBuilder();
    }

    #region Error

    [TestMethod]
    public void SetAttributes_SetsEmptyAttributes()
    {
        List<Attribute> attributes = new List<Attribute>();

        _interfaceBuilderTest!.SetAttributes(attributes);
        var result = _interfaceBuilderTest.GetResult();

        result.Attributes.Should().BeEmpty();
    }

    #endregion
}
