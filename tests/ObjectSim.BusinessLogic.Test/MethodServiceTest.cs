using FluentAssertions;
using Moq;
using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;
using ValueType = ObjectSim.Domain.ValueType;

namespace ObjectSim.BusinessLogic.Test;

[TestClass]
public class MethodServiceTest
{
    private Mock<IRepository<Method>>? _methodRepositoryMock;
    private Mock<IRepository<Class>>? _classRepositoryMock;
    private Mock<IDataTypeService>? _dataTypeServiceMock;
    private MethodService? _methodService;

    private static readonly Guid ClassId = Guid.NewGuid();
    private static readonly Guid MethodId = Guid.NewGuid();

    private static readonly ReferenceType TestLocalVariable = new ReferenceType("TestLocalVariable", "string", []);

    private static readonly ValueType TestParameter = new ValueType("TestParameter", "int", []);

    private readonly CreateMethodArgs _testCreateMethodArgs = new CreateMethodArgs(
        "TestMethod",
        new CreateDataTypeArgs("TestParameter", "int"),
        "public",
        false,
        false,
        false,
        ClassId,
        [],
        [],
        []
    );

    private readonly Method? _testMethod = new Method
    {
        Id = MethodId,
        Name = "TestMethod",
        Type = new ValueType("MethodType", "int", []),
        Abstract = false,
        IsSealed = false,
        Accessibility = Method.MethodAccessibility.Public,
        Parameters = [],
        LocalVariables = [],
        MethodsInvoke = []
    };

    [TestInitialize]
    public void Initialize()
    {
        _methodRepositoryMock = new Mock<IRepository<Method>>(MockBehavior.Strict);
        _classRepositoryMock = new Mock<IRepository<Class>>(MockBehavior.Strict);
        _dataTypeServiceMock = new Mock<IDataTypeService>(MockBehavior.Strict);
        _methodService = new MethodService(_methodRepositoryMock.Object, _classRepositoryMock.Object, _dataTypeServiceMock.Object);
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
        Action act = () => _methodService!.CreateMethod(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void CreateMethod_WithEmptyArgs_ThrowsException()
    {
        var emptyArgs = new CreateMethodArgs("", null, "", null, null, null, Guid.Empty, [], [], []);

        Action act = () => _methodService!.CreateMethod(emptyArgs);

        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void CreateMethod_WhenMethodAlreadyExistsInClass_ThrowsException()
    {
        _classServiceMock!.Setup(cs => cs.GetById(It.IsAny<Guid>()))
            .Returns(new Class { Id = ClassId });

        _classServiceMock!.Setup(cs => cs.AddMethod(It.IsAny<Guid>(), It.IsAny<Method>()))
            .Throws(new InvalidOperationException());

        Action act = () => _methodService!.CreateMethod(_testCreateMethodArgs);

        act.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void CreateMethod_WhenMethodInvokeMethodsDosentExists_ThrowsException()
    {
        _classServiceMock!.Setup(cs => cs.GetById(It.IsAny<Guid>()))
            .Returns(new Class { Id = ClassId, Name = "TestClass" });

        _methodRepositoryMock!.Setup(repo => repo.Get(It.IsAny<Func<Method, bool>>()))
            .Returns((Method)null!);

        _testCreateMethodArgs.InvokeMethods = [Guid.NewGuid()];

        _methodService!.CreateMethod(_testCreateMethodArgs);
    }

    #endregion

    #region Success

    [TestMethod]
    public void CreateMethod_WhenValid_ReturnsNewMethodAndAddItToDataBase()
    {
        _testCreateMethodArgs.Name = "TestMethod";
        //_testCreateMethodArgs.Type = "int";
        _testCreateMethodArgs.ClassId = ClassId;
        _testCreateMethodArgs.Accessibility = _testCreateMethodArgs.Accessibility;
        _testCreateMethodArgs.IsAbstract = false;
        _testCreateMethodArgs.IsSealed = false;
        _testCreateMethodArgs.IsOverride = false;
        _testCreateMethodArgs.LocalVariables = [];
        _testCreateMethodArgs.Parameters = [];
        _testCreateMethodArgs.InvokeMethods = _testCreateMethodArgs.InvokeMethods;

        _classServiceMock!.Setup(cs => cs.GetById(It.IsAny<Guid>()))
            .Returns(new Class { Id = ClassId, Name = "TestClass" });

        _classServiceMock.Setup(cs => cs.AddMethod(It.IsAny<Guid>(), It.IsAny<Method>()));

        _methodRepositoryMock!.Setup(repo => repo.Add(It.IsAny<Method>())).Returns((Method act) => act);

        var result = _methodService!.CreateMethod(_testCreateMethodArgs);

        result.Should().NotBeNull();
        result.Id.Should().NotBe(Guid.Empty);
        result.Name.Should().Be(_testCreateMethodArgs.Name);
        //result.Type.Should().Be("int");
        result.Accessibility.Should().Be(Method.MethodAccessibility.Public);
        result.ClassId.Should().Be(ClassId);
        result.IsOverride.Should().BeFalse();
    }

    [TestMethod]
    public void CreateMethod_WhenIsValidHasListInvokeMethods_ReturnsNewMethodAndAddItToDataBase()
    {
        _testCreateMethodArgs.InvokeMethods = [Guid.NewGuid()];

        _classServiceMock!.Setup(cs => cs.GetById(It.IsAny<Guid>()))
            .Returns(new Class { Id = ClassId, Name = "TestClass" });

        _methodRepositoryMock!.Setup(repo => repo.Get(It.IsAny<Func<Method, bool>>()))
            .Returns(new Method());

        _classServiceMock.Setup(cs => cs.AddMethod(It.IsAny<Guid>(), It.IsAny<Method>()));

        _methodRepositoryMock!.Setup(repo => repo.Add(It.IsAny<Method>())).Returns((Method act) => act);

        var result = _methodService!.CreateMethod(_testCreateMethodArgs);

        result.Should().NotBeNull();
        result.MethodsInvoke.Count.Should().Be(1);
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
        _testClass.Methods!.Add(_testMethod);

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(_testClass);

        Action action = () => _classServiceTest!.AddMethod(_testClass.Id, _testMethod);
        action.Should().Throw<ArgumentException>("Method already exists in class.");
    }

    [TestMethod]
    public void AddMethod_ClassIsInterfaceMethodIsSealed_ThrowsException()
    {
        _testMethod.IsSealed = true;

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(_testInterfaceClass);

        Action action = () => _classServiceTest!.AddMethod(_testInterfaceClass.Id, _testMethod);
        action.Should().Throw<ArgumentException>("Method cannot be sealed in an interface.");
    }

    [TestMethod]
    public void AddMethod_ClassIsInterfaceMethodIsOverriding_ThrowsException()
    {
        _testMethod.IsOverride = true;

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(_testInterfaceClass);

        Action action = () => _classServiceTest!.AddMethod(_testInterfaceClass.Id, _testMethod);
        action.Should().Throw<ArgumentException>("Method cannot be overridden in an interface.");
    }

    [TestMethod]
    public void AddMethod_ClassIsInterfaceMethodAccesibilityIsPrivate_ThrowsException()
    {
        _testMethod.Accessibility = Method.MethodAccessibility.Private;

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(_testInterfaceClass);

        Action action = () => _classServiceTest!.AddMethod(_testInterfaceClass.Id, _testMethod);
        action.Should().Throw<ArgumentException>("Method cannot be private in an interface.");
    }

    [TestMethod]
    public void AddMethod_ClassIsInterfaceMethodThatHaveLocalVariables_ThrowsException()
    {
        _testMethod.LocalVariables = [new ValueType("variable", "int", [])];

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(_testInterfaceClass);

        Action action = () => _classServiceTest!.AddMethod(_testInterfaceClass.Id, _testMethod);
        action.Should().Throw<ArgumentException>("Method cannot be implemented in an interface.");
    }

    [TestMethod]
    public void AddMethod_ClassIsInterfaceMethodThatHaveMethodInvoke_ThrowsException()
    {
        _testMethod.MethodsInvoke = [new Method()];

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(_testInterfaceClass);

        Action action = () => _classServiceTest!.AddMethod(_testInterfaceClass.Id, _testMethod);
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
            Parameters = [new ValueType("variable", "int", [])]
        };

        var newMethod = new Method
        {
            Name = "TestMethod2",
            Parameters = [new ValueType("variable", "int", [])]
        };

        _testClass.Methods!.Add(existingMethod);

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(_testClass);

        _classServiceTest!.AddMethod(classId, newMethod);

        _testClass.Methods.Should().Contain(newMethod);
    }

    [TestMethod]
    public void AddMethod_WithSameParamsInDifferentOrder_AddsMethod()
    {
        var classId = Guid.NewGuid();

        var existingMethod = new Method
        {
            Name = "TestMethod",
            Parameters = [
                new ValueType("variable", "int", []),
                new ValueType("variableTwo", "bool", [])
            ]
        };

        var newMethod = new Method
        {
            Name = "TestMethod",
            Parameters = [
                new ValueType("variableTwo", "bool", []),
                new ValueType("variable", "int", [])
            ]
        };

        _testClass.Methods!.Add(existingMethod);

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(_testClass);

        _classServiceTest!.AddMethod(classId, newMethod);

        _testClass.Methods.Should().Contain(newMethod);
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
            Parameters = [new ValueType("variable", "bool", [])]
        };

        var newMethod = new Method
        {
            Name = "TestMethod",
            Abstract = false,
            IsSealed = false,
            Parameters = [new ValueType("variable", "int", [])]
        };

        _testClass.Methods!.Add(existingMethod);

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(_testClass);

        _classServiceTest!.AddMethod(classId, newMethod);

        _testClass.Methods.Should().Contain(newMethod);
    }

    [TestMethod]
    public void AddMethod_WithDifferentParameterCount_AddsMethod()
    {
        var classId = Guid.NewGuid();

        var existingMethod = new Method
        {
            Name = "TestMethod",
            Parameters = [new ValueType("variable", "bool", [])]
        };

        var newMethod = new Method
        {
            Name = "TestMethod",
            Parameters = [
                new ValueType("variable", "bool", []),
                new ValueType("variableTwo", "int", [])
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

        _classServiceTest!.AddMethod(classId, newMethod);

        testClass.Methods.Should().Contain(newMethod);
    }

    [TestMethod]
    public void AddMethod_TryingToAddOverridingParentMethod_AddsMethod()
    {
        var classId = Guid.NewGuid();

        var existingMethod = new Method
        {
            Name = "TestMethod",
            Parameters = [new ValueType("variable", "int", [])]
        };

        var newMethod = new Method
        {
            Name = "TestMethod",
            Parameters = [
                new ValueType("variable", "int", []),
            ],
            IsOverride = true
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
    public void AddMethod_ClassIsInterfaceMethodIsNotAbstract_MakeMethodAbstractAndAddsMethod()
    {
        _testMethod.Abstract = false;

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(_testInterfaceClass);

        _classServiceTest!.AddMethod(_testInterfaceClass.Id, _testMethod);

        _testInterfaceClass.Methods.Should().Contain(_testMethod);
    }

    [TestMethod]
    public void AddMethod_ClassIsInterfaceValidMethod_AddsMethod()
    {
        var validMethod = new Method
        {
            Name = "TestMethod",
            Abstract = true,
            IsSealed = false,
            Parameters = [],
        };

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(_testInterfaceClass);

        _classServiceTest!.AddMethod(_testInterfaceClass.Id, validMethod);

        _testInterfaceClass.Methods.Should().Contain(validMethod);
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

        _methodService!.GetAll();
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void GetAllMethods_WhenNoMethods_ShouldThrowException()
    {
        _methodRepositoryMock!.Setup(repo => repo.GetAll(It.IsAny<Func<Method, bool>>())).Returns([]);
        _methodService!.GetAll();
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

        var result = _methodService!.GetAll();

        result.Should().HaveCount(2);
        _methodRepositoryMock.Verify(repo => repo.GetAll(It.IsAny<Func<Method, bool>>()), Times.Once);
    }

    #endregion

    #endregion

    #region GetById-Method-Test

    #region Error

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void GetById_WhenMethodNotFound_ShouldThrowInvalidOperationException()
    {
        _methodRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Method, bool>>()))
            .Throws(new Exception());

        _methodService!.GetById(Guid.NewGuid());
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void DeleteMethod_WhenMethodNotFound_ShouldThrowException()
    {
        _methodRepositoryMock!
            .Setup(x => x.Get(It.IsAny<Func<Method, bool>>()))
            .Returns((Method)null!);
        _methodService!.Delete(ClassId);
    }

    #endregion

    #region Success

    [TestMethod]
    public void GetById_WhenValid_ShouldReturnMethod()
    {
        _methodRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Method, bool>>()))
            .Returns(_testMethod!);

        var result = _methodService!.GetById(ClassId);

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

        var result = _methodService!.Delete(_testMethod!.Id);

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
            Type = new ValueType("TestParameter", "int", []),
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

        var result = _methodService!.Update(_testMethod!.Id, newMethod);

        result.Should().NotBeNull();
        result.Id.Should().Be(_testMethod.Id);
        result.Name.Should().Be(newMethod.Name);
        result.Type.Should().Be(newMethod.Type);
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
            Type = new ValueType("TestParameter", "int", []),
            Abstract = true,
            IsSealed = false,
            Accessibility = Method.MethodAccessibility.Private,
            Parameters = [],
            LocalVariables = []
        };

        _methodRepositoryMock!
            .Setup(x => x.Get(It.Is<Func<Method, bool>>(f => f(_testMethod!))))
            .Returns(_testMethod);

        _methodService!.Update(_testMethod.Id, invalidUpdate);
    }

    #endregion

    #region Add-Parameter-Test

    #region Error

    [TestMethod]
    public void AddParameter_WhenMethodNotFound_ShouldThrow()
    {
        _methodRepositoryMock!.Setup(r => r.Get(It.IsAny<Func<Method, bool>>()))
            .Returns((Method?)null);

        Action act = () => _methodService!.AddParameter(_testMethod!.Id, TestParameter);

        act.Should().Throw<Exception>().WithMessage("Method not found");

        _methodRepositoryMock.Verify(r => r.Get(It.IsAny<Func<Method, bool>>()), Times.Once);
    }

    [TestMethod]
    public void AddParameter_WhenDuplicate_ShouldThrow()
    {
        var existing = new ValueType("variable", "bool", []);
        var param = new ValueType("variable", "bool", []);

        _testMethod!.Parameters = [existing];

        _methodRepositoryMock!.Setup(r => r.Get(It.IsAny<Func<Method, bool>>()))
            .Returns(_testMethod);

        Action act = () => _methodService!.AddParameter(_testMethod.Id, param);

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

        var result = _methodService!.AddParameter(_testMethod!.Id, TestParameter);

        result.Should().NotBeNull();
        result.Name.Should().Be(TestParameter.Name);
        _testMethod.Parameters.Should().ContainSingle(p => p.Name == TestParameter.Name);
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

        Action act = () => _methodService!.AddLocalVariable(_testMethod!.Id, TestLocalVariable);

        act.Should().Throw<Exception>().WithMessage("Method not found");
    }

    [TestMethod]
    public void AddLocalVariable_WhenDuplicateName_ShouldThrow()
    {
        var existing = new ValueType("variable", "bool", []);
        var newVar = new ValueType("variable", "bool", []);

        _testMethod!.LocalVariables = [existing];

        _methodRepositoryMock!.Setup(r => r.Get(It.IsAny<Func<Method, bool>>()))
            .Returns(_testMethod);

        Action act = () => _methodService!.AddLocalVariable(_testMethod.Id, newVar);

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

        var result = _methodService!.AddLocalVariable(_testMethod!.Id, TestLocalVariable);

        result.Should().NotBeNull();
        result.Name.Should().Be(TestLocalVariable.Name);
        _testMethod!.LocalVariables.Should().ContainSingle(v => v.Name == TestLocalVariable.Name);
    }

    #endregion

    #endregion
}
