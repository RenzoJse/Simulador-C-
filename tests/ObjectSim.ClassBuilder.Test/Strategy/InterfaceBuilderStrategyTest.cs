using ObjectSim.ClassLogic.ClassBuilders.Builders;
using ObjectSim.ClassLogic.Strategy;
using ObjectSim.Domain.Args;

namespace ObjectSim.ClassLogic.Test.Strategy;

[TestClass]
public class InterfaceBuilderStrategyTest
{
    private InterfaceBuilderStrategy? _strategy;

    [TestInitialize]
    public void Initialize()
    {
        _strategy = new InterfaceBuilderStrategy();
    }

    #region WhichIsMyBuilder

    [TestMethod]
    public void WhichIsMyBuilder_WithNormalClass_ReturnsFalse()
    {
        var args = new CreateClassArgs("NormalClass", false, false, false, [], [], null);

        var result = _strategy!.WhichIsMyBuilder(args);

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void WhichIsMyBuilder_WithInterfaceClass_ReturnsTrue()
    {
        var args = new CreateClassArgs("InterfaceClass", false, false, true, [], [], null);

        var result = _strategy!.WhichIsMyBuilder(args);

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void WhichIsMyBuilder_WithAbstractClass_ReturnsFalse()
    {
        var args = new CreateClassArgs("AbstractClass", true, false, false, [], [], null);

        var result = _strategy!.WhichIsMyBuilder(args);

        Assert.IsFalse(result);
    }

    #endregion

    #region CreateBuilder

    [TestMethod]
    public void CreateBuilder_WhenIsInterface_ReturnsInterfaceBuilder()
    {
        var builder = _strategy!.CreateBuilder();

        Assert.IsNotNull(builder);
        Assert.IsInstanceOfType(builder, typeof(InterfaceBuilder));
    }

    #endregion
}
