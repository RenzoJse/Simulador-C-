using Moq;
using ObjectSim.BusinessLogic.ClassLogic.ClassBuilders.Builders;
using ObjectSim.BusinessLogic.ClassLogic.Strategy;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.BusinessLogic.Test.ClassLogic.Strategy;

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

        _strategy = new ClassBuilderStrategy(_methodServiceMock.Object);
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

    [TestMethod]
    public void CreateBuilder_ReturnsClassBuilderInstance()
    {
        var builder = _strategy!.CreateBuilder(_classServiceMock!.Object, _attributeServiceMock!.Object);

        Assert.IsNotNull(builder);
        Assert.IsInstanceOfType(builder, typeof(ClassBuilder));
    }
}
