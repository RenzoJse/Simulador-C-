using FluentAssertions;
using Moq;
using ObjectSim.BusinessLogic.ClassLogic;
using ObjectSim.BusinessLogic.ClassLogic.ClassBuilders;
using ObjectSim.BusinessLogic.ClassLogic.ClassBuilders.Builders;
using ObjectSim.BusinessLogic.ClassLogic.Strategy;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;
using Attribute = System.Attribute;

namespace ObjectSim.BusinessLogic.Test.ClassLogic;

[TestClass]
public class ClassServiceTest
{
    private ClassService? _classServiceTest;
    private Mock<IBuilderStrategy>? _builderStrategyMock;
    private Mock<IAttributeService>? _attributeServiceMock;
    private Mock<IMethodService>? _methodServiceMock;

    //private static readonly Builder ClassBuilder = new ClassBuilder(null!, null!, null!);

    private static readonly CreateClassArgs Args = new CreateClassArgs("TestClass",
        false,
        false,
        false,
        [],
        [],
        Guid.NewGuid());

    protected virtual Builder GetMockedBuilder()
    {
        var methodServiceMock = new Mock<IMethodService>();
        var classServiceMock = new Mock<IClassService>();
        var attributeServiceMock = new Mock<IAttributeService>();

        return new ClassBuilder(
            methodServiceMock.Object,
            classServiceMock.Object,
            attributeServiceMock.Object);
    }


    [TestInitialize]
    public void Initialize()
    {
        _methodServiceMock = new Mock<IMethodService>(MockBehavior.Strict);
        _attributeServiceMock = new Mock<IAttributeService>(MockBehavior.Strict);
        _builderStrategyMock = new Mock<IBuilderStrategy>(MockBehavior.Strict);

        var strategies = new List<IBuilderStrategy> { _builderStrategyMock!.Object };

        _classServiceTest = new ClassService(strategies);
    }

    #region CreateClass

    #region Error

    [TestMethod]
    public void CreateClass_WithEmptyName_ThrowsException()
    {
        Action action = () => new ClassService(null!).CreateClass(Args);

        action.Should().Throw<ArgumentNullException>();
    }


    #endregion

    #region Success

    [TestMethod]
    public void CreateClass_WithValidParameters_SetsStrategyBuilder()
    {
        var classBuilder = GetMockedBuilder();

        _builderStrategyMock!.Setup(x => x.WhichIsMyBuilder(Args)).Returns(true);
        _builderStrategyMock.Setup(x => x.CreateBuilder()).Returns(classBuilder);

        Action action = () => _classServiceTest!.CreateClass(Args);

        action.Should().NotThrow();
    }

    [TestMethod]
    public void CreateClass_WithNullParent_LeavesNullParent()
    {
        Action action = () => new ClassService(null!).CreateClass(null!);

        action.Should().Throw<ArgumentNullException>();
    }

    #endregion

    #endregion
}
