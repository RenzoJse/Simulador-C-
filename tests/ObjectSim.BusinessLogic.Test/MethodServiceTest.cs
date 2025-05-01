using FluentAssertions;
using Moq;
using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.BusinessLogic.Test;

[TestClass]
public class MethodServiceTest
{
    private Mock<IRepository<Method>>? _methodRepositoryMock;
    private Mock<IClassService>? _classServiceMock;
    private MethodService? _methodService;
    private Method? _testMethod;

    private static readonly Guid ClassId = Guid.NewGuid();
    private static readonly Guid MethodId = Guid.NewGuid();


    private static readonly LocalVariable TestLocalVariable = new LocalVariable
    {
        Name = "TestLocalVariable",
        Type = LocalVariable.LocalVariableDataType.String,
    };

    private static readonly Parameter TestParameter = new Parameter
    {
        Name = "TestParameter",
        Type = Parameter.ParameterDataType.String,
    };

    private readonly CreateMethodArgs _testCreateMethodArgs = new CreateMethodArgs(
        "TestMethod",
        "string",
        "public",
        false,
        false,
        false,
        ClassId,
        [],
        [],
        []
    );

    [TestInitialize]
    public void Initialize()
    {
        _methodRepositoryMock = new Mock<IRepository<Method>>(MockBehavior.Strict);
        _classServiceMock = new Mock<IClassService>(MockBehavior.Strict);
        _methodService = new MethodService(_methodRepositoryMock.Object, _classServiceMock.Object);
        _testMethod = new Method
        {
            Id = MethodId,
            Name = "TestMethod",
            Type = Method.MethodDataType.String,
            Abstract = false,
            IsSealed = false,
            Accessibility = Method.MethodAccessibility.Public,
            Parameters = [],
            LocalVariables = [],
            MethodsInvoke = []
        };
    }

    [TestCleanup]
    public void CleanUp()
    {
        _methodRepositoryMock!.VerifyAll();
        _classServiceMock!.VerifyAll();
    }

    #region CreateMethod

    #region Error

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CreateMethod_WithNullArgs_ThrowsException()
    {
        _methodService!.CreateMethod(null!);
    }

    [TestMethod]
    public void CreateMethod_WithEmptyArgs_ThrowsException()
    {
        var emptyArgs = new CreateMethodArgs("", "", "", null, null, null, Guid.Empty, [], [], []);

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
        _testCreateMethodArgs.Type = "";
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
        result.Type.Should().Be(Method.MethodDataType.String);
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


    #region GetAll-Methods-Test
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

    #region GetById-Method-Test
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
        _methodRepositoryMock.Verify(x => x.Delete(It.Is<Method>(m => m.Id == _testMethod.Id)), Times.Once);
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
            Type = Method.MethodDataType.String,
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

        _methodRepositoryMock.Verify(x => x.Get(It.IsAny<Func<Method, bool>>()), Times.Once);
        _methodRepositoryMock.Verify(x => x.Update(It.IsAny<Method>()), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void UpdateMethod_WhenNameIsEmpty_ShouldThrowException()
    {
        var invalidUpdate = new Method
        {
            Id = _testMethod!.Id,
            Name = "",
            Type = Method.MethodDataType.String,
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

    [TestMethod]
    public void AddParameter_WhenValid_ShouldAdd()
    {
        var param = new Parameter
        {
            Name = "parameterTest",
            Type = Parameter.ParameterDataType.String
        };

        _methodRepositoryMock!.Setup(r => r.Get(It.IsAny<Func<Method, bool>>()))
            .Returns(_testMethod);

        _methodRepositoryMock!.Setup(r => r.Update(It.IsAny<Method>()))
            .Returns((Method m) => m);

        var result = _methodService!.AddParameter(_testMethod!.Id, TestParameter);

        result.Should().NotBeNull();
        result.Name.Should().Be(TestParameter.Name);
        _testMethod.Parameters.Should().ContainSingle(p => p.Name == TestParameter.Name);
        _methodRepositoryMock.Verify(r => r.Update(_testMethod), Times.Once);
    }

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
        var existing = new Parameter
        {
            Name = "parameterTest",
            Type = Parameter.ParameterDataType.String
        };

        var param = new Parameter
        {
            Name = "parameterTest",
            Type = Parameter.ParameterDataType.String
        };

        _testMethod!.Parameters = [existing];

        _methodRepositoryMock!.Setup(r => r.Get(It.IsAny<Func<Method, bool>>()))
            .Returns(_testMethod);

        Action act = () => _methodService!.AddParameter(_testMethod.Id, param);

        act.Should().Throw<Exception>().WithMessage("Parameter already exists in this method");

        _methodRepositoryMock.Verify(r => r.Get(It.IsAny<Func<Method, bool>>()), Times.Once);
    }

    #endregion

    #region Add-LocalVariable-Test

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
        _methodRepositoryMock.Verify(r => r.Update(_testMethod), Times.Once);
    }

    [TestMethod]
    public void AddLocalVariable_WhenMethodNotFound_ShouldThrow()
    {
        _methodRepositoryMock!.Setup(r => r.Get(It.IsAny<Func<Method, bool>>()))
            .Returns((Method?)null);

        Action act = () => _methodService!.AddLocalVariable(_testMethod!.Id, TestLocalVariable);

        act.Should().Throw<Exception>().WithMessage("Method not found");

        _methodRepositoryMock.Verify(r => r.Get(It.IsAny<Func<Method, bool>>()), Times.Once);
    }

    [TestMethod]
    public void AddLocalVariable_WhenDuplicateName_ShouldThrow()
    {
        var existing = new LocalVariable
        {
            Name = "lvTest1",
            Type = LocalVariable.LocalVariableDataType.Bool
        };

        var newVar = new LocalVariable
        {
            Name = "lvTest1",
            Type = LocalVariable.LocalVariableDataType.Bool
        };

        _testMethod!.LocalVariables = [existing];

        var methodRepoMock = new Mock<IRepository<Method>>(MockBehavior.Strict);
        methodRepoMock.Setup(r => r.Get(It.IsAny<Func<Method, bool>>()))
            .Returns(_testMethod);

        _methodService = new MethodService(methodRepoMock.Object, _classServiceMock!.Object);

        Action act = () => _methodService.AddLocalVariable(_testMethod.Id, newVar);

        act.Should().Throw<Exception>().WithMessage("LocalVariable already exists in this method");

        methodRepoMock.Verify(r => r.Get(It.IsAny<Func<Method, bool>>()), Times.Once);
    }

    #endregion
}
