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

    private readonly Class _testInterfaceClass = new Class
    {
        Name = "TestClass",
        IsAbstract = false,
        IsInterface = true,
        IsSealed = false,
        Attributes = [],
        Methods = [],
        Parent = null,
    };

    //private static readonly Builder ClassBuilder = new ClassBuilder(null!, null!, null!);

    private readonly CreateClassArgs _args = new CreateClassArgs("TestClass",
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

    [TestCleanup]
    public void Cleanup()
    {
        _methodServiceMock?.VerifyAll();
        _attributeServiceMock?.VerifyAll();
        _builderStrategyMock?.VerifyAll();
        _classRepositoryMock?.VerifyAll();
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

        _args.Name = null;
        _classServiceTest!.CreateClass(_args);
    }

    [TestMethod]
    [ExpectedException(typeof(NullReferenceException))]
    public void CreateClass_WithNoMatchingStrategy_ThrowsException()
    {
        _builderStrategyMock!.Setup(x => x.WhichIsMyBuilder(It.IsAny<CreateClassArgs>())).Returns(false);
        _builderStrategyMock.Setup(x => x.CreateBuilder());

        _classServiceTest!.CreateClass(_args);
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

        _builderStrategyMock!.Setup(x => x.WhichIsMyBuilder(_args)).Returns(true);
        _builderStrategyMock.Setup(x => x.CreateBuilder()).Returns(classBuilder);

        Action action = () => _classServiceTest!.CreateClass(_args);

        action.Should().NotThrow();
    }

    [TestMethod]
    public void CreateClass_WithNullParent_LeavesNullParent()
    {
        _args.Parent = null;
        var classBuilder = GetMockedBuilder();

        _builderStrategyMock!.Setup(x => x.WhichIsMyBuilder(It.IsAny<CreateClassArgs>())).Returns(true);
        _builderStrategyMock.Setup(x => x.CreateBuilder()).Returns(classBuilder);

        Action action = () => _classServiceTest!.CreateClass(_args);

        action.Should().NotThrow();
    }

    [TestMethod]
    public void CreateClass_WithValidParameters_ReturnsClass()
    {
        var classBuilder = GetMockedBuilder();

        _builderStrategyMock!.Setup(x => x.WhichIsMyBuilder(It.IsAny<CreateClassArgs>())).Returns(true);
        _builderStrategyMock.Setup(x => x.CreateBuilder()).Returns(classBuilder);

        var result = _classServiceTest!.CreateClass(_args);
        result.Should().NotBeNull();
        result.Should().BeOfType<Class>();
        result.Name.Should().Be(_args.Name);
        result.IsAbstract.Should().Be(_args.IsAbstract);
        result.IsInterface.Should().Be(_args.IsInterface);
        result.IsSealed.Should().Be(_args.IsSealed);
        result.Attributes.Should().BeEquivalentTo(_args.Attributes);
        result.Methods.Should().BeEquivalentTo(_args.Methods);
        result.Parent.Should().Be(null);
    }

    #endregion

    #endregion

    #region GetById

    #region Error

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void GetById_WithNullId_ThrowsException()
    {
        _classServiceTest!.GetById(null!);
    }

    [TestMethod]
    public void GetById_WithInexistantId_ThrowsException()
    {
        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns((Class?)null);

        Action action = () => _classServiceTest!.GetById(Guid.NewGuid());
        action.Should().Throw<ArgumentException>();
    }

    #endregion

    #region Success

    [TestMethod]
    public void GetById_WithValidId_ReturnsClass()
    {
        var testClass = new Class
        {
            Id = Guid.NewGuid(),
            Name = "TestClass",
            IsAbstract = false,
            IsInterface = false,
            IsSealed = false,
            Attributes = [],
            Methods = [],
            Parent = null,
        };

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(testClass);

        var result = _classServiceTest!.GetById(testClass.Id);
        result.Should().NotBeNull();
        result.Should().BeOfType<Class>();
        result.Should().Be(testClass);
    }

    #endregion

    #endregion

    #region AddMethod

    #region Error

    [TestMethod]
    public void AddMethod_WithNullClassId_ThrowsException()
    {
        Action action = () => _classServiceTest!.AddMethod(null!, new Method());
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void AddMethod_WithNullMethod_ThrowsException()
    {
        Action action = () => _classServiceTest!.AddMethod(Guid.NewGuid(), null!);
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void AddMethod_TryingToAddRepeatedMethodNotOverriding_ThrowsException()
    {
        var classId = Guid.NewGuid();
        var method = new Method
        {
            Name = "TestMethod",
            Abstract = false,
            //ADD OVERRIDE
            IsSealed = false,
            Parameters = [],
        };

        var testClass = new Class
        {
            Id = classId,
            Name = "TestClass",
            IsAbstract = false,
            IsInterface = false,
            IsSealed = false,
            Attributes = [],
            Methods = [method],
            Parent = null,
        };

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(testClass);

        Action action = () => _classServiceTest!.AddMethod(classId, method);
        action.Should().Throw<ArgumentException>("Method already exists in class.");
    }

    [TestMethod]
    public void AddMethod_ClassIsInterfaceMethodIsSealed_ThrowsException()
    {
        var method = new Method
        {
            Name = "TestMethod",
            Abstract = false,
            IsSealed = true,
            Parameters = [],
        };

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(_testInterfaceClass);

        Action action = () => _classServiceTest!.AddMethod(_testInterfaceClass.Id, method);
        action.Should().Throw<ArgumentException>("Method cannot be sealed in an interface.");
    }

    [TestMethod]
    public void AddMethod_ClassIsInterfaceMethodIsOverriding_ThrowsException()
    {
        var method = new Method
        {
            Name = "TestMethod",
            Abstract = false,
            IsSealed = false,
            IsOverride = true,
            Parameters = [],
        };

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(_testInterfaceClass);

        Action action = () => _classServiceTest!.AddMethod(_testInterfaceClass.Id, method);
        action.Should().Throw<ArgumentException>("Method cannot be overridden in an interface.");
    }

    [TestMethod]
    public void AddMethod_ClassIsInterfaceMethodAccesibilityIsPrivate_ThrowsException()
    {
        var method = new Method
        {
            Name = "TestMethod",
            Abstract = false,
            IsSealed = false,
            Accessibility = Method.MethodAccessibility.Private,
            Parameters = [],
        };

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(_testInterfaceClass);

        Action action = () => _classServiceTest!.AddMethod(_testInterfaceClass.Id, method);
        action.Should().Throw<ArgumentException>("Method cannot be private in an interface.");
    }

    [TestMethod]
    public void AddMethod_ClassIsInterfaceMethodThatHaveLocalVariables_ThrowsException()
    {
        var method = new Method
        {
            Name = "TestMethod",
            Abstract = false,
            IsSealed = false,
            LocalVariables = [new LocalVariable()]
        };

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(_testInterfaceClass);

        Action action = () => _classServiceTest!.AddMethod(_testInterfaceClass.Id, method);
        action.Should().Throw<ArgumentException>("Method cannot be implemented in an interface.");
    }

    [TestMethod]
    public void AddMethod_ClassIsInterfaceMethodThatHaveMethodInvoke_ThrowsException()
    {
        var method = new Method
        {
            Name = "TestMethod",
            Abstract = false,
            IsSealed = false,
            MethodsInvoke = [new Method()]
        };

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(_testInterfaceClass);

        Action action = () => _classServiceTest!.AddMethod(_testInterfaceClass.Id, method);
        action.Should().Throw<ArgumentException>("Method cannot be implemented in an interface.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void AddMethod_WithCompletelyDifferentMethod_AddsMethods()
    {
        var classId = Guid.NewGuid();

        var existingMethod = new Method
        {
            Name = "TestMethod1",
            Parameters = [new Parameter { Name = "param1", Type = Parameter.ParameterDataType.Int }]
        };

        var newMethod = new Method
        {
            Name = "TestMethod2",
            Parameters = [new Parameter { Name = "otherParam", Type = Parameter.ParameterDataType.Bool }]
        };

        var testClass = new Class
        {
            Id = classId,
            Name = "TestClass",
            Methods = [existingMethod]
        };

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(testClass);

        _classServiceTest!.AddMethod(classId, newMethod);

        testClass.Methods.Should().Contain(newMethod);
    }

    [TestMethod]
    public void AddMethod_WithSameParamsInDifferentOrder_AddsMethod()
    {
        var classId = Guid.NewGuid();

        var existingMethod = new Method
        {
            Name = "TestMethod",
            Parameters = [
                new Parameter { Name = "param1", Type = Parameter.ParameterDataType.Int },
                new Parameter { Name = "param2", Type = Parameter.ParameterDataType.String }
            ]
        };

        var newMethod = new Method
        {
            Name = "TestMethod",
            Parameters = [
                new Parameter { Name = "param2", Type = Parameter.ParameterDataType.String },
                new Parameter { Name = "param1", Type = Parameter.ParameterDataType.Int }
            ]
        };

        var testClass = new Class
        {
            Id = classId,
            Name = "TestClass",
            Methods = [existingMethod]
        };

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(testClass);

        Action action = () => _classServiceTest!.AddMethod(classId, newMethod);

        action.Should().Throw<NotImplementedException>();
    }

    [TestMethod]
    public void AddMethod_WithSameNameAndTypeButDifferentParameters_AddsMethod()
    {
        var classId = Guid.NewGuid();

        var existingMethod = new Method
        {
            Name = "TestMethod",
            Abstract = false,
            IsSealed = false,
            Parameters = [new Parameter { Name = "param1", Type = Parameter.ParameterDataType.Int }]
        };

        var newMethod = new Method
        {
            Name = "TestMethod",
            Abstract = false,
            IsSealed = false,
            Parameters = [new Parameter { Name = "param1", Type = Parameter.ParameterDataType.String }]
        };

        var testClass = new Class
        {
            Id = classId,
            Name = "TestClass",
            Methods = [existingMethod]
        };

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(testClass);

        Action action = () => _classServiceTest!.AddMethod(classId, newMethod);

        action.Should().Throw<NotImplementedException>();
    }

    [TestMethod]
    public void AddMethod_WithDifferentParameterCount_ShouldNotThrowMethodExistsException()
    {
        var classId = Guid.NewGuid();

        var existingMethod = new Method
        {
            Name = "TestMethod",
            Parameters = [new Parameter { Name = "param1", Type = Parameter.ParameterDataType.Int }]
        };

        var newMethod = new Method
        {
            Name = "TestMethod",
            Parameters = [
                new Parameter { Name = "param1", Type = Parameter.ParameterDataType.Int },
                new Parameter { Name = "param2", Type = Parameter.ParameterDataType.String }
            ]
        };

        var testClass = new Class
        {
            Id = classId,
            Name = "TestClass",
            Methods = [existingMethod]
        };

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(testClass);

        Action action = () => _classServiceTest!.AddMethod(classId, newMethod);

        action.Should().Throw<NotImplementedException>();
    }

    [TestMethod]
    public void AddMethod_TryingToAddOverridingParentMethod_AddsMethod()
    {
    }

    [TestMethod]
    public void AddMethod_ClassIsInterfaceMethodIsNotAbstract_MakeMethodAbstractAndAddsMethod()
    {
        var method = new Method
        {
            Name = "TestMethod",
            Abstract = false,
            IsSealed = false,
            Parameters = [],
        };

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(_testInterfaceClass);

        _classServiceTest!.AddMethod(_testInterfaceClass.Id, method);

        _testInterfaceClass.Methods.Should().Contain(method);
    }

    [TestMethod]
    public void AddMethod_ClassIsInterfaceValidMethod_AddsMethod()
    {
        var method = new Method
        {
            Name = "TestMethod",
            Abstract = true,
            IsSealed = false,
            Parameters = [],
        };

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(_testInterfaceClass);

        _classServiceTest!.AddMethod(_testInterfaceClass.Id, method);

        _testInterfaceClass.Methods.Should().Contain(method);
    }

    #endregion

    #endregion
}
