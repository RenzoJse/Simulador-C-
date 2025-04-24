using FluentAssertions;
using Moq;
using ObjectSim.BusinessLogic.ClassLogic;
using ObjectSim.BusinessLogic.ClassLogic.ClassBuilders;
using ObjectSim.BusinessLogic.ClassLogic.ClassBuilders.Builders;
using ObjectSim.BusinessLogic.ClassLogic.Strategy;
using ObjectSim.DataAccess.Interface;
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
    private Mock<IClassService>? _classServiceMock;
    private Mock<IMethodService>? _methodServiceMock;
    private Mock<IRepository<Class>>? _classRepositoryMock;

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
        _methodServiceMock = new Mock<IMethodService>();
        _classServiceMock = new Mock<IClassService>();
        _attributeServiceMock = new Mock<IAttributeService>();

        return new ClassBuilder(
            _methodServiceMock.Object,
            _classServiceMock.Object,
            _attributeServiceMock.Object);
    }

    [TestInitialize]
    public void Initialize()
    {
        _methodServiceMock = new Mock<IMethodService>(MockBehavior.Strict);
        _attributeServiceMock = new Mock<IAttributeService>(MockBehavior.Strict);
        _builderStrategyMock = new Mock<IBuilderStrategy>(MockBehavior.Strict);
        _classRepositoryMock = new Mock<IRepository<Class>>(MockBehavior.Strict);

        var strategies = new List<IBuilderStrategy> { _builderStrategyMock!.Object };

        _classServiceTest = new ClassService(strategies, _classRepositoryMock.Object);
    }

    #region CreateClass

    #region Error

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CreateClass_WithNullArgs_ThrowsException()
    {
       _classServiceTest!.CreateClass(null!);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CreateClass_WithEmptyName_ThrowsException()
    {
        var classBuilder = GetMockedBuilder();

        _builderStrategyMock!.Setup(x => x.WhichIsMyBuilder(It.IsAny<CreateClassArgs>())).Returns(true);
        _builderStrategyMock.Setup(x => x.CreateBuilder()).Returns(classBuilder);

        Args.Name = null;
        _classServiceTest!.CreateClass(Args);
    }

    [TestMethod]
    [ExpectedException(typeof(NullReferenceException))]
    public void CreateClass_WithNoMatchingStrategy_ThrowsException()
    {
        _builderStrategyMock!.Setup(x => x.WhichIsMyBuilder(It.IsAny<CreateClassArgs>())).Returns(false);
        _builderStrategyMock.Setup(x => x.CreateBuilder());

        _classServiceTest!.CreateClass(Args);
    }


    [TestMethod]
    public void CreateClass_WithNullAttributesList_ThrowsArgumentException()
    {
        var argsWithNullAttributes = new CreateClassArgs(
            "TestClass",
            false,
            false,
            false,
            null!,
            [],
            Guid.NewGuid());

        var classBuilder = GetMockedBuilder();
        _builderStrategyMock!.Setup(x => x.WhichIsMyBuilder(It.IsAny<CreateClassArgs>())).Returns(true);
        _builderStrategyMock.Setup(x => x.CreateBuilder()).Returns(classBuilder);

        Action action = () => _classServiceTest!.CreateClass(argsWithNullAttributes);

        action.Should().Throw<ArgumentException>();
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
        Args.Parent = null;
        var classBuilder = GetMockedBuilder();

        _builderStrategyMock!.Setup(x => x.WhichIsMyBuilder(It.IsAny<CreateClassArgs>())).Returns(true);
        _builderStrategyMock.Setup(x => x.CreateBuilder()).Returns(classBuilder);

        Action action = () => _classServiceTest!.CreateClass(Args);

        action.Should().NotThrow();
    }

    [TestMethod]
    public void CreateClass_WithValidParameters_ReturnsClass()
    {
        var classBuilder = GetMockedBuilder();

        _builderStrategyMock!.Setup(x => x.WhichIsMyBuilder(It.IsAny<CreateClassArgs>())).Returns(true);
        _builderStrategyMock.Setup(x => x.CreateBuilder()).Returns(classBuilder);

        var result = _classServiceTest!.CreateClass(Args);
        result.Should().NotBeNull();
        result.Should().BeOfType<Class>();
        result.Name.Should().Be(Args.Name);
        result.IsAbstract.Should().Be(Args.IsAbstract);
        result.IsInterface.Should().Be(Args.IsInterface);
        result.IsSealed.Should().Be(Args.IsSealed);
        result.Attributes.Should().BeEquivalentTo(Args.Attributes);
        result.Methods.Should().BeEquivalentTo(Args.Methods);
        result.Parent.Should().Be(null);
    }

    #endregion

    #endregion
}
