using Moq;
using ObjectSim.ClassLogic.ClassBuilders.Builders;
using ObjectSim.ClassLogic.Strategy;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.ClassLogic.Test.Strategy;

[TestClass]
public class ClassBuilderStrategyTest
{
    private Mock<IMethodService>? _methodServiceMock;
    private Mock<IAttributeService>? _attributeServiceMock;
    private Mock<IClassService>? _classServiceMock;
    private ClassBuilderStrategy? _strategy;

    [TestInitialize]
    public void Initialize()
    {
        _methodServiceMock = new Mock<IMethodService>();
        _attributeServiceMock = new Mock<IAttributeService>();
        _classServiceMock = new Mock<IClassService>();

        _strategy = new ClassBuilderStrategy(_methodServiceMock.Object, _attributeServiceMock!.Object);
    }

    #region WhichIsMyBuilder

    [TestMethod]
    public void WhichIsMyBuilder_WithNormalClass_ReturnsTrue()
    {
        var args = new CreateClassArgs("NormalClass", false, false, false, [], [], null);

        var result = _strategy!.WhichIsMyBuilder(args);

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void WhichIsMyBuilder_WithInterfaceClass_ReturnsFalse()
    {
        var args = new CreateClassArgs("InterfaceClass", false, false, true, [], [], null);

        var result = _strategy!.WhichIsMyBuilder(args);

        Assert.IsFalse(result);
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
    public void CreateBuilder_WhenIsNormalClass_ReturnsClassBuilder()
    {
        var builder = _strategy!.CreateBuilder();

        Assert.IsNotNull(builder);
        Assert.IsInstanceOfType(builder, typeof(ClassBuilder));
    }

    #endregion
}
