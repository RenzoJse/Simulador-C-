using FluentAssertions;
using Moq;
using Moq.Language;
using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;
using Attribute = ObjectSim.Domain.Attribute;
using ValueType = ObjectSim.Domain.ValueType;

namespace ObjectSim.BusinessLogic.Test;

[TestClass]
public class MethodServiceTest
{
    private Mock<IRepository<Method>>? _methodRepositoryMock;
    private Mock<IRepository<Class>>? _classRepositoryMock;
    private Mock<IDataTypeService>? _dataTypeServiceMock;
    private Mock<IRepository<Variable>>? _variableRepositoryMock;
    private Mock<IInvokeMethodService>? _invokeMethodServiceMock;
    private MethodService? _methodServiceTest;

    private static readonly Guid ClassId = Guid.NewGuid();
    private static readonly Guid MethodId = Guid.NewGuid();

    private readonly CreateMethodArgs _testCreateMethodArgs = new(
        "TestMethod",
        Guid.NewGuid(),
        "public",
        false,
        false,
        false,
        false,
        false,
        ClassId,
        [],
        [],
        []
    );

    private Method? _testMethod;
    private Variable? _testLocalVariable;
    private Variable? _testParameter;

    [TestInitialize]
    public void Initialize()
    {
        _testMethod = new Method
        {
            Id = MethodId,
            Name = "TestMethod",
            TypeId = Guid.NewGuid(),
            Abstract = false,
            IsSealed = false,
            Accessibility = Method.MethodAccessibility.Public,
            Parameters = [],
            LocalVariables = [],
            MethodsInvoke = []
        };

        _testLocalVariable = new Variable(Guid.NewGuid(), "string", _testMethod);
        _testParameter =  new Variable(Guid.NewGuid(), "int", _testMethod);

        _methodRepositoryMock = new Mock<IRepository<Method>>(MockBehavior.Strict);
        _classRepositoryMock = new Mock<IRepository<Class>>(MockBehavior.Strict);
        _dataTypeServiceMock = new Mock<IDataTypeService>(MockBehavior.Strict);
        _invokeMethodServiceMock = new Mock<IInvokeMethodService>(MockBehavior.Strict);
        _variableRepositoryMock = new Mock<IRepository<Variable>>(MockBehavior.Strict);
        _methodServiceTest = new MethodService(_variableRepositoryMock.Object,
            _methodRepositoryMock.Object,
            _classRepositoryMock.Object,
            _dataTypeServiceMock.Object,
            _invokeMethodServiceMock.Object);
    }

    [TestCleanup]
    public void CleanUp()
    {
        _methodRepositoryMock!.VerifyAll();
        _classRepositoryMock!.VerifyAll();
        _dataTypeServiceMock!.VerifyAll();
    }

    #region CreateMethod

    #region Error

    [TestMethod]
    public void CreateMethod_WithNullArgs_ThrowsException()
    {
        Action act = () => _methodServiceTest!.CreateMethod(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void CreateMethod_WithEmptyArgs_ThrowsException()
    {
        var emptyArgs = new CreateMethodArgs("", Guid.Empty, "", null, null, null, null, false, Guid.Empty, [], [], []);

        Action act = () => _methodServiceTest!.CreateMethod(emptyArgs);

        act.Should().Throw<ArgumentException>();
    }

    #endregion

    #region Success

    [TestMethod]
    public void CreateMethod_WhenValid_ReturnsNewMethodAndAddItToDataBase()
    {
        var classObj = new Class
        {
            Id = ClassId,
            Name = "TestClass",
            Methods = [],
            Attributes = []
        };

        _classRepositoryMock!.Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(classObj);

        _dataTypeServiceMock!.Setup(service => service.GetById(It.IsAny<Guid>()))
            .Returns(new ValueType(Guid.NewGuid(), "int"));

        _methodRepositoryMock!.Setup(repo => repo.Add(It.IsAny<Method>()))
            .Returns((Method m) => m);

        var result = _methodServiceTest!.CreateMethod(_testCreateMethodArgs);

        result.Should().NotBeNull();
        result.Name.Should().Be(_testCreateMethodArgs.Name);
        result.TypeId.Should().NotBeEmpty();
    }

    [TestMethod]
    public void CreateMethod_WhenInvokeMethodsProvided_ShouldCallSetMethodInvokes()
    {
        var classObj = new Class
        {
            Id = ClassId,
            Name = "TestClass",
            Methods = [],
            Attributes = []
        };

        var invokeMethodArg = new CreateInvokeMethodArgs(Guid.NewGuid(), "ref");
        var args = new CreateMethodArgs(
            "TestMethodWithInvoke",
            Guid.NewGuid(),
            "public",
            false,
            false,
            false,
            false,
            false,
            ClassId,
            [],
            [],
            [invokeMethodArg]
        );

        _classRepositoryMock!.Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(classObj);

        _dataTypeServiceMock!.Setup(service => service.GetById(It.IsAny<Guid>()))
            .Returns(new ValueType(Guid.NewGuid(), "int"));

        Method? createdMethod = null;
        _methodRepositoryMock!.Setup(repo => repo.Add(It.IsAny<Method>()))
            .Returns((Method m) => { createdMethod = m; return m; });

        _methodRepositoryMock!.Setup(repo => repo.Get(It.IsAny<Func<Method, bool>>()))
            .Returns((Func<Method, bool> predicate) =>
            {
                if (createdMethod != null && predicate(createdMethod))
                {
                    return createdMethod;
                }
                var methodToInvoke = new Method { Id = invokeMethodArg.InvokeMethodId, Name = "InvokedMethod", ClassId = ClassId, Parameters = [], LocalVariables = [], MethodsInvoke = [] };
                return predicate(methodToInvoke) ? methodToInvoke : null;
            });

        _invokeMethodServiceMock!
            .Setup(s => s.CreateInvokeMethod(It.IsAny<CreateInvokeMethodArgs>(), It.IsAny<Method>()))
            .Returns(new InvokeMethod(invokeMethodArg.InvokeMethodId, MethodId, invokeMethodArg.Reference));

        var result = _methodServiceTest!.CreateMethod(args);

        result.Should().NotBeNull();
    }

    #endregion

    #endregion

    #region GetAll-Methods-Test

    #region Error

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void GetAllMethods_ShouldThrowException_WhenRepositoryFails()
    {
        _methodRepositoryMock!.Setup(repo => repo.GetAll(It.IsAny<Func<Method, bool>>())).Throws(new Exception());

        _methodServiceTest!.GetAll();
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void GetAllMethods_WhenNoMethods_ShouldThrowException()
    {
        _methodRepositoryMock!.Setup(repo => repo.GetAll(It.IsAny<Func<Method, bool>>())).Returns([]);
        _methodServiceTest!.GetAll();
    }

    #endregion

    #region Success

    [TestMethod]
    public void GetAllMethods_ShouldReturnMethods()
    {
        var methods = new List<Method>
        {
            new Method { Name = "m1Test", Abstract = false, IsSealed = false, Accessibility = Method.MethodAccessibility.Public, LocalVariables = [], Parameters = [], MethodsInvoke = [] },
            new Method { Name = "m2Test", Abstract = false, IsSealed = false, Accessibility = Method.MethodAccessibility.Public, LocalVariables = [], Parameters = [], MethodsInvoke = [] }
        };

        _methodRepositoryMock!.Setup(repo => repo.GetAll(It.IsAny<Func<Method, bool>>())).Returns(methods);

        var result = _methodServiceTest!.GetAll();

        result.Should().HaveCount(2);
        _methodRepositoryMock.Verify(repo => repo.GetAll(It.IsAny<Func<Method, bool>>()), Times.Once);
    }

    #endregion

    #endregion

    #region GetById-Method-Test

    #region Error

    [TestMethod]
    public void GetById_WhenMethodNotFound_ShouldThrowInvalidOperationException()
    {
        _methodRepositoryMock!
            .Setup(x => x.Get(It.IsAny<Func<Method, bool>>()))
            .Returns((Method)null!);

        Action act = () => _methodServiceTest!.GetById(ClassId);

        act.Should().Throw<KeyNotFoundException>()
            .WithMessage($"Method with ID {ClassId} not found.");
    }

    [TestMethod]
    [ExpectedException(typeof(KeyNotFoundException))]
    public void DeleteMethod_WhenMethodNotFound_ShouldThrowException()
    {
        _methodRepositoryMock!
            .Setup(x => x.Get(It.IsAny<Func<Method, bool>>()))
            .Returns((Method)null!);
        _methodServiceTest!.Delete(ClassId);
    }

    [TestMethod]
    public void CreateMethod_WhenClassNotFound_ShouldThrowArgumentException()
    {
        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns((Class?)null);

        _dataTypeServiceMock!
            .Setup(service => service.GetById(It.IsAny<Guid>()))
            .Returns(new ValueType(Guid.NewGuid(), "int"));

        var args = new CreateMethodArgs("Test", Guid.NewGuid(), "public", false, false, false, false, false, Guid.NewGuid(), [], [], []);

        Action act = () => _methodServiceTest!.CreateMethod(args);

        act.Should().Throw<ArgumentException>().WithMessage("Class not found.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void GetById_WhenValid_ShouldReturnMethod()
    {
        _methodRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Method, bool>>()))
            .Returns(_testMethod!);

        var result = _methodServiceTest!.GetById(ClassId);

        result.Should().NotBeNull();
        result.Id.Should().Be(_testMethod!.Id);
        result.Name.Should().Be(_testMethod.Name);
    }

    #endregion

    #endregion

    #region Delete-Method-Test

    [TestMethod]
    public void DeleteMethod_WhenExists_ShouldDeleteAndReturnTrue()
    {
        _methodRepositoryMock!
            .Setup(x => x.Get(It.IsAny<Func<Method, bool>>()))
            .Returns(_testMethod!);

        _methodRepositoryMock!
            .Setup(x => x.Delete(It.IsAny<Method>()));

        var result = _methodServiceTest!.Delete(_testMethod!.Id);

        result.Should().BeTrue();
    }

    #endregion

    #region Update-Method-Test

    [TestMethod]
    public void UpdateMethod_WhenValid_ShouldReturnUpdatedMethod()
    {
        var newMethod = new Method
        {
            Id = ClassId,
            Name = "UpdatedMethod",
            TypeId = Guid.NewGuid(),
            Abstract = true,
            IsSealed = true,
            Accessibility = Method.MethodAccessibility.Private,
            Parameters = [],
            LocalVariables = []
        };

        _methodRepositoryMock!
            .Setup(x => x.Get(It.Is<Func<Method, bool>>(filter => filter(_testMethod!))))
            .Returns(_testMethod);

        _methodRepositoryMock!
            .Setup(x => x.Update(It.IsAny<Method>()))
            .Returns((Method m) => m);

        var result = _methodServiceTest!.Update(_testMethod!.Id, newMethod);

        result.Should().NotBeNull();
        result.Id.Should().Be(_testMethod.Id);
        result.Name.Should().Be(newMethod.Name);
        result.TypeId.Should().Be(newMethod.TypeId);
        result.Abstract.Should().Be(newMethod.Abstract);
        result.IsSealed.Should().Be(newMethod.IsSealed);
        result.Accessibility.Should().Be(newMethod.Accessibility);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void UpdateMethod_WhenNameIsEmpty_ShouldThrowException()
    {
        var invalidUpdate = new Method
        {
            Id = _testMethod!.Id,
            Name = "",
            TypeId = Guid.NewGuid(),
            Abstract = true,
            IsSealed = false,
            Accessibility = Method.MethodAccessibility.Private,
            Parameters = [],
            LocalVariables = []
        };

        _methodRepositoryMock!
            .Setup(x => x.Get(It.Is<Func<Method, bool>>(f => f(_testMethod!))))
            .Returns(_testMethod);

        _methodServiceTest!.Update(_testMethod.Id, invalidUpdate);
    }

    #endregion

    #region Add-Parameter-Test

    #region Error

    [TestMethod]
    public void AddParameter_WhenMethodNotFound_ShouldThrow()
    {
        _methodRepositoryMock!.Setup(r => r.Get(It.IsAny<Func<Method, bool>>()))
            .Returns((Method?)null);

        Action act = () => _methodServiceTest!.AddParameter(_testMethod!.Id, _testParameter!);

        act.Should().Throw<Exception>()
            .WithMessage($"Method with ID {_testMethod!.Id} not found.");
    }

    [TestMethod]
    public void AddParameter_WhenDuplicate_ShouldThrow()
    {
        var existing = new Variable(Guid.NewGuid(), "bool", _testMethod!);
        var param = new Variable(Guid.NewGuid(), "bool", _testMethod!);

        _testMethod!.Parameters = [existing];

        _methodRepositoryMock!.Setup(r => r.Get(It.IsAny<Func<Method, bool>>()))
            .Returns(_testMethod);

        Action act = () => _methodServiceTest!.AddParameter(_testMethod.Id, param);

        act.Should().Throw<Exception>().WithMessage("Parameter already exists in this method");
    }

    #endregion

    #region Success

    [TestMethod]
    public void AddParameter_WhenValid_ShouldAdd()
    {
        _methodRepositoryMock!.Setup(r => r.Get(It.IsAny<Func<Method, bool>>()))
            .Returns(_testMethod);

        _methodRepositoryMock!.Setup(r => r.Update(It.IsAny<Method>()))
            .Returns((Method m) => m);

        var result = _methodServiceTest!.AddParameter(_testMethod!.Id, _testParameter!);

        result.Should().NotBeNull();
        result.Name.Should().Be(_testParameter!.Name);
        _testMethod.Parameters.Should().ContainSingle(p => p.Name == _testParameter.Name);
    }

    #endregion

    #endregion

    #region Add-LocalVariable-Test

    #region Error

    [TestMethod]
    public void AddLocalVariable_WhenMethodNotFound_ShouldThrow()
    {
        _methodRepositoryMock!.Setup(r => r.Get(It.IsAny<Func<Method, bool>>()))
            .Returns((Method?)null);

        Action act = () => _methodServiceTest!.AddLocalVariable(_testMethod!.Id, _testLocalVariable!);

        act.Should().Throw<Exception>()
            .WithMessage($"Method with ID {_testMethod!.Id} not found.");
    }

    [TestMethod]
    public void AddLocalVariable_WhenDuplicateName_ShouldThrow()
    {
        var existing = new Variable(Guid.NewGuid(), "bool", _testMethod!);
        var newVar = new Variable(Guid.NewGuid(), "bool", _testMethod!);

        _testMethod!.LocalVariables = [existing];

        _methodRepositoryMock!.Setup(r => r.Get(It.IsAny<Func<Method, bool>>()))
            .Returns(_testMethod);

        Action act = () => _methodServiceTest!.AddLocalVariable(_testMethod.Id, newVar);

        act.Should().Throw<Exception>().WithMessage("LocalVariable already exists in this method");
    }

    #endregion

    #region Success

    [TestMethod]
    public void AddLocalVariable_WhenValid_ShouldAddToMethod()
    {
        _methodRepositoryMock!.Setup(r => r.Get(It.IsAny<Func<Method, bool>>()))
            .Returns(_testMethod);

        _methodRepositoryMock!.Setup(r => r.Update(It.IsAny<Method>()))
            .Returns((Method m) => m);

        var result = _methodServiceTest!.AddLocalVariable(_testMethod!.Id, _testLocalVariable!);

        result.Should().NotBeNull();
        result.Name.Should().Be(_testLocalVariable!.Name);
        _testMethod!.LocalVariables.Should().ContainSingle(v => v.Name == _testLocalVariable.Name);
    }

    #endregion

    #endregion

    #region AddInvokeMethod

    #region Error

    [TestMethod]
    public void AddInvokeMethod_WhenMethodNotFound_ThrowsException()
    {
        var invokeMethodArgs = new List<CreateInvokeMethodArgs>([new CreateInvokeMethodArgs(Guid.NewGuid(), "init")]);

        _methodRepositoryMock!.Setup(r => r.Get(It.IsAny<Func<Method, bool>>()))
            .Returns((Method?)null);

        Action act = () => _methodServiceTest!.AddInvokeMethod(_testMethod!.Id, invokeMethodArgs);

        act.Should().Throw<Exception>()
            .WithMessage($"Method with ID {_testMethod!.Id} not found.");
    }

    [TestMethod]
    public void AddInvokeMethod_WhenInvokeMethodDoNotExist_ThrowsException()
    {
        var methodToInvokeId = Guid.NewGuid();
        var invokeMethodArgs = new List<CreateInvokeMethodArgs> { new(methodToInvokeId, "init") };

        ISetupSequentialResult<Method?> setupSequence =
            _methodRepositoryMock!.SetupSequence(r => r.Get(It.IsAny<Func<Method, bool>>()));

        setupSequence.Returns(_testMethod);

        setupSequence.Returns((Method?)null);

        Action act = () => _methodServiceTest!.AddInvokeMethod(_testMethod!.Id, invokeMethodArgs);

        act.Should().Throw<Exception>()
            .WithMessage($"Method to invoke with id {methodToInvokeId} not found");
    }

    [TestMethod]
    public void AddInvokeMethod_WhenInvokeMethodCannotBeAdded_ThrowsException()
    {
        var invokeMethod = new Method { Id = Guid.NewGuid(), Name = "test", Parameters = [] };
        var invokeMethodArgs = new List<CreateInvokeMethodArgs> { new(invokeMethod.Id, "init") };

        ISetupSequentialResult<Method?> setupSequence =
            _methodRepositoryMock!.SetupSequence(r => r.Get(It.IsAny<Func<Method, bool>>()));

        setupSequence.Returns(_testMethod);

        setupSequence.Returns(invokeMethod);

        _classRepositoryMock!.Setup(r => r.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(new Class { Methods = [], Attributes = [] });

        Action act = () => _methodServiceTest!.AddInvokeMethod(_testMethod!.Id, invokeMethodArgs);

        act.Should().Throw<Exception>();
    }

    [TestMethod]
    public void AddInvokeMethod_WhenInvokeMethodListEmpty_ThrowsException()
    {
        var invokeMethodArgs = new List<CreateInvokeMethodArgs>();

        Action act = () => _methodServiceTest!.AddInvokeMethod(_testMethod!.Id, invokeMethodArgs);

        act.Should().Throw<ArgumentException>()
            .WithMessage("Invoke method arguments cannot be null or empty.");
    }

    [TestMethod]
    public void AddInvokeMethod_WhenInvokeMethodIsNotReachable_ThrowsException()
    {
        var invokeMethod = new Method
        {
            Id = Guid.NewGuid(),
            Name = "methodToInvoke",
            ClassId = Guid.NewGuid()
        };

        var method = new Method
        {
            Id = Guid.NewGuid(),
            Name = "test",
            ClassId = Guid.NewGuid(),
            Parameters = [],
            LocalVariables = [],
            MethodsInvoke = []
        };

        var invokeMethodArgs = new List<CreateInvokeMethodArgs>
        {
            new(invokeMethod.Id, "this")
        };

        ISetupSequentialResult<Method?> setupSequence =
            _methodRepositoryMock!.SetupSequence(r => r.Get(It.IsAny<Func<Method, bool>>()));

        setupSequence.Returns(method);
        setupSequence.Returns(invokeMethod);

        _classRepositoryMock!.Setup(r => r.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(new Class
            {
                Id = method.ClassId,
                Methods = [],
                Attributes = []
            });

        Action act = () => _methodServiceTest!.AddInvokeMethod(method.Id, invokeMethodArgs);

        act.Should().Throw<Exception>();
    }

    [TestMethod]
    public void AddInvokeMethod_WhenArgsNull_ThrowsArgumentException()
    {
        Action act = () => _methodServiceTest!.AddInvokeMethod(Guid.NewGuid(), null!);
        act.Should().Throw<ArgumentException>().WithMessage("Invoke method arguments cannot be null or empty.");
    }

    [TestMethod]
    public void AddInvokeMethod_WhenArgsEmpty_ThrowsArgumentException()
    {
        Action act = () => _methodServiceTest!.AddInvokeMethod(Guid.NewGuid(), []);
        act.Should().Throw<ArgumentException>().WithMessage("Invoke method arguments cannot be null or empty.");
    }

    [TestMethod]
    public void AddInvokeMethod_WhenInvokeMethodNotFound_ThrowsException()
    {
        var method = new Method { Id = Guid.NewGuid(), Name = "m", ClassId = Guid.NewGuid(), Parameters = [], LocalVariables = [], MethodsInvoke = [] };
        _methodRepositoryMock!.SetupSequence(r => r.Get(It.IsAny<Func<Method, bool>>()))
            .Returns(method)
            .Returns((Method?)null);
        var args = new List<CreateInvokeMethodArgs> { new(Guid.NewGuid(), "ref") };
        Action act = () => _methodServiceTest!.AddInvokeMethod(method.Id, args);
        act.Should().Throw<Exception>();
    }

    [TestMethod]
    public void AddInvokeMethod_WhenStaticReferenceIsWrong_ThrowsArgumentException()
    {
        var classId = Guid.NewGuid();
        var method = new Method { Id = Guid.NewGuid(), Name = "m", ClassId = classId, Parameters = [], LocalVariables = [], MethodsInvoke = [] };
        var invokeMethod = new Method { Id = Guid.NewGuid(), Name = "toInvoke", ClassId = Guid.NewGuid(), IsStatic = true, Accessibility = Method.MethodAccessibility.Public };
        var classOfInvoke = new Class { Id = invokeMethod.ClassId, Name = "OtherClass", Methods = [invokeMethod], Attributes = [] };

        _methodRepositoryMock!.SetupSequence(r => r.Get(It.IsAny<Func<Method, bool>>()))
            .Returns(method)
            .Returns(invokeMethod);

        _classRepositoryMock!.Setup(r => r.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(classOfInvoke);

        var args = new List<CreateInvokeMethodArgs> { new(invokeMethod.Id, "WrongRef") };

        Action act = () => _methodServiceTest!.AddInvokeMethod(method.Id, args);

        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void AddInvokeMethod_WhenInvokeMethodIsNotReachable_ThrowsArgumentException()
    {
        var method = new Method { Id = Guid.NewGuid(), Name = "m", ClassId = Guid.NewGuid(), Parameters = [], LocalVariables = [], MethodsInvoke = [] };
        var invokeMethod = new Method { Id = Guid.NewGuid(), Name = "toInvoke", ClassId = Guid.NewGuid(), Accessibility = Method.MethodAccessibility.Private };

        _methodRepositoryMock!.SetupSequence(r => r.Get(It.IsAny<Func<Method, bool>>()))
            .Returns(method)
            .Returns(invokeMethod);

        _classRepositoryMock!.Setup(r => r.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(new Class { Id = method.ClassId, Methods = [], Attributes = [] });

        var args = new List<CreateInvokeMethodArgs> { new(invokeMethod.Id, "ref") };

        Action act = () => _methodServiceTest!.AddInvokeMethod(method.Id, args);

        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void AddInvokeMethod_WhenInvokeFromOtherClassWithInvalidReference_ThrowsException()
    {
        var methodId = Guid.NewGuid();
        var classAId = Guid.NewGuid();
        var classBId = Guid.NewGuid();
        var invokeMethodId = Guid.NewGuid();
        const string reference = "WrongClass";

        var method = new Method
        {
            Id = methodId,
            Name = "main",
            ClassId = classAId,
            Parameters = [],
            LocalVariables = []
        };

        var invokeMethod = new Method
        {
            Id = invokeMethodId,
            Name = "OtherMethod",
            ClassId = classBId,
            IsStatic = true,
            Accessibility = Method.MethodAccessibility.Public
        };

        var classOfMethod = new Class
        {
            Id = classAId,
            Name = "ClassA",
            Methods = [method]
        };

        var classOfInvokeMethod = new Class
        {
            Id = classBId,
            Name = "ClassB"
        };

        _methodRepositoryMock!.Setup(r => r.Get(It.IsAny<Func<Method, bool>>()))
            .Returns((Func<Method, bool> filter) =>
            {
                var all = new[] { method, invokeMethod };
                return all.FirstOrDefault(filter);
            });

        _classRepositoryMock!.Setup(r => r.Get(It.IsAny<Func<Class, bool>>()))
            .Returns((Func<Class, bool> filter) =>
            {
                var all = new[] { classOfMethod, classOfInvokeMethod };
                return all.FirstOrDefault(filter);
            });

        var args = new List<CreateInvokeMethodArgs>
        {
            new(invokeMethodId, reference)
        };

        Action act = () => _methodServiceTest!.AddInvokeMethod(methodId, args);

        act.Should().Throw<ArgumentException>()
            .WithMessage($"Cant invoke static method OtherMethod from class ClassB using reference {reference}");
    }

    #endregion

    #region Success

    [TestMethod]
    public void AddInvokeMethod_WhenInvokeIsValid_AddsInvokeMethod()
    {
        var testMethodId = Guid.NewGuid();
        var classId = Guid.NewGuid();

        var testMethod = new Method
        {
            Id = testMethodId,
            Name = "MétodoPrincipal",
            ClassId = classId,
            Parameters = [],
            LocalVariables = [],
            MethodsInvoke = []
        };

        var invokeMethod = new Method
        {
            Id = Guid.NewGuid(),
            Name = "test",
            Parameters = [],
            ClassId = classId,
            MethodsInvoke = []
        };

        var invokeMethodArgs = new List<CreateInvokeMethodArgs> { new(invokeMethod.Id, "this") };

        _methodRepositoryMock!.Setup(r => r.Get(It.Is<Func<Method, bool>>(f => f(testMethod))))
            .Returns(testMethod);

        _methodRepositoryMock!.Setup(r => r.Get(It.Is<Func<Method, bool>>(f => f(invokeMethod))))
            .Returns(invokeMethod);

        _classRepositoryMock!.Setup(r => r.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(new Class { Id = classId, Methods = [invokeMethod, testMethod], Attributes = [] });

        _invokeMethodServiceMock!
            .Setup(s => s.CreateInvokeMethod(It.IsAny<CreateInvokeMethodArgs>(), It.IsAny<Method>()))
            .Returns((CreateInvokeMethodArgs args, Method m) =>
            {
                var invoke = new InvokeMethod(args.InvokeMethodId, m.Id, args.Reference);
                m.MethodsInvoke.Add(invoke);
                return invoke;
            });

        Method result = _methodServiceTest!.AddInvokeMethod(testMethod.Id, invokeMethodArgs);

        result.Should().NotBeNull();
        result.MethodsInvoke.Should().ContainSingle(m => m.MethodId == invokeMethod.Id);
    }

    [TestMethod]
    public void AddInvokeMethod_WhenMethodExistsInClass_ShouldSucceed()
    {
        var methodId = Guid.NewGuid();
        var invokeMethodId = Guid.NewGuid();
        const string reference = "obj";

        var method = new Method
        {
            Id = methodId,
            Name = "MainMethod",
            ClassId = Guid.NewGuid(),
            Parameters = [],
            LocalVariables = []
        };

        var classOfMethod = new Class
        {
            Id = method.ClassId,
            Name = "MainClass",
            Methods = [method]
        };

        var invokeMethod = new Method
        {
            Id = invokeMethodId,
            ClassId = classOfMethod.Id,
            Name = "InvokedMethod",
            Accessibility = Method.MethodAccessibility.Public,
            IsStatic = false
        };

        _methodRepositoryMock!.Setup(r => r.Get(It.IsAny<Func<Method, bool>>()))
            .Returns((Func<Method, bool> filter) =>
            {
                var all = new[] { method, invokeMethod };
                return all.FirstOrDefault(filter);
            });

        _classRepositoryMock!.Setup(r => r.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(classOfMethod);

        _invokeMethodServiceMock!.Setup(s => s.CreateInvokeMethod(It.IsAny<CreateInvokeMethodArgs>(), method))
            .Returns(new InvokeMethod(Guid.NewGuid(), methodId, reference));

        var args = new List<CreateInvokeMethodArgs>
        {
            new(invokeMethodId, reference)
        };

        var result = _methodServiceTest!.AddInvokeMethod(methodId, args);

        result.Should().Be(method);
    }

    [TestMethod]
    public void AddInvokeMethod_WhenMethodInParentOfAttributeClass_Succeeds()
    {
        var methodId = Guid.NewGuid();
        var invokeMethodId = Guid.NewGuid();
        const string reference = "myAttr";

        var parentClassId = Guid.NewGuid();
        var attributeClassId = Guid.NewGuid();
        var mainClassId = Guid.NewGuid();

        var method = new Method
        {
            Id = methodId,
            Name = "main",
            ClassId = mainClassId,
            Parameters = [],
            LocalVariables = [],
            MethodsInvoke = [],
        };

        var invokeMethod = new Method
        {
            Id = invokeMethodId,
            ClassId = parentClassId,
            Name = "InvokedMethod",
            IsStatic = false,
            Accessibility = Method.MethodAccessibility.Public,
            MethodsInvoke = []
        };

        var attributeClass = new Class
        {
            Id = attributeClassId, Name = "AttrClass", Parent = new Class { Id = parentClassId }
        };

        var parentClass = new Class
        {
            Id = parentClassId, Name = "ParentClass", Methods = [invokeMethod]
        };

        var mainClass = new Class
        {
            Id = mainClassId,
            Name = "MainClass",
            Attributes = [new Attribute { Name = reference, ClassId = attributeClassId }],
            Methods = [method]
        };

        Class[] allClasses = [mainClass, attributeClass, parentClass];

        _methodRepositoryMock!.Setup(r => r.Get(It.IsAny<Func<Method, bool>>()))
            .Returns((Func<Method, bool> filter) =>
            {
                Method[] all = [method, invokeMethod];
                return all.FirstOrDefault(filter);
            });

        _classRepositoryMock!.Setup(r => r.Get(It.IsAny<Func<Class, bool>>()))
            .Returns((Func<Class, bool> filter) => allClasses.FirstOrDefault(filter));

        _invokeMethodServiceMock!.Setup(s => s.CreateInvokeMethod(It.IsAny<CreateInvokeMethodArgs>(), method))
            .Returns((CreateInvokeMethodArgs _, Method _) => new InvokeMethod(Guid.NewGuid(), methodId, reference));

        var args = new List<CreateInvokeMethodArgs> { new(invokeMethodId, reference) };

        Method result = _methodServiceTest!.AddInvokeMethod(methodId, args);

        result.Should().Be(method);
    }

    #endregion

    #endregion

    #region SystemMethod

    #region Error

    [TestMethod]
    public void GetIdByName_WhenMethodDoesNotExist_ThrowsArgumentException()
    {
        const string methodName = "NonExistentMethod";
        var methods = new List<Method>();

        _methodRepositoryMock!
            .Setup(repo => repo.GetAll(It.IsAny<Func<Method, bool>>()))
            .Returns(methods);

        Action act = () => _methodServiceTest!.GetIdByName(methodName);

        act.Should().Throw<ArgumentException>().WithMessage("Method not found");
    }

    #endregion

    #region Success

    [TestMethod]
    public void GetIdByName_WhenMethodExists_ReturnsMethod()
    {
        const string methodName = "TestMethod";
        var method = new Method { Id = Guid.NewGuid(), Name = methodName };
        var methods = new List<Method> { method };

        _methodRepositoryMock!
            .Setup(repo => repo.GetAll(It.IsAny<Func<Method, bool>>()))
            .Returns(methods);

        var result = _methodServiceTest!.GetIdByName(methodName);

        result.Should().NotBeNull();
        result.Name.Should().Be(methodName);
    }

    #endregion

    #endregion

    #region CreateMethodBuilder

    #region Error

    [TestMethod]
    public void BuilderCreateMethod_WithNullArgs_ThrowsArgumentNullException()
    {
        Action act = () => _methodServiceTest!.BuilderCreateMethod(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void BuilderCreateMethod_WithEmptyTypeId_ThrowsArgumentException()
    {
        var args = new CreateMethodArgs("Test", Guid.Empty, "public", false, false, false, false, false, Guid.NewGuid(),
            [], [], []);
        Action act = () => _methodServiceTest!.BuilderCreateMethod(args);
        act.Should().Throw<ArgumentException>().WithMessage("Name ID cannot be empty.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void BuilderCreateMethod_WhenValidArgs_ReturnsMethod()
    {
        var args = new CreateMethodArgs("Test", Guid.NewGuid(), "public", false, false, false, false, false,
            Guid.NewGuid(), [], [], []);
        _dataTypeServiceMock!.Setup(s => s.GetById(It.IsAny<Guid>())).Returns(new ValueType(Guid.NewGuid(), "int"));

        Method result = _methodServiceTest!.BuilderCreateMethod(args);

        result.Should().NotBeNull();
        result.Name.Should().Be("Test");
        result.LocalVariables.Should().BeEmpty();
        result.Parameters.Should().BeEmpty();
    }

    #endregion

    #endregion
}
