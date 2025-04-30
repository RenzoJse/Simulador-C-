using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;

namespace ObjectSim.BusinessLogic.Test;

[TestClass]
public class MethodServiceTest
{
    private Mock<IRepository<Method>>? _methodRepositoryMock;
    private MethodService? _methodService;
    private static readonly Guid ClassId = Guid.NewGuid();
    private Method? _testMethod;
    private CreateMethodArgs _testCreateMethodArgs = new CreateMethodArgs(
        "TestMethod",
        "string",
        "public",
        false,
        false,
        false,
        [],
        [],
        []
    );

    [TestInitialize]
    public void Initialize()
    {
        _methodRepositoryMock = new Mock<IRepository<Method>>(MockBehavior.Strict);
        _methodService = new MethodService(_methodRepositoryMock.Object);
        _testMethod = new Method
        {
            Id = ClassId,
            Name = "MetodoDePrueba",
            Type = Method.MethodDataType.String,
            Abstract = false,
            IsSealed = false,
            Accessibility = Method.MethodAccessibility.Public,
            Parameters = [],
            LocalVariables = [],
            MethodsInvoke = []
        };
    }

    #region CreateMethod

    #region Error

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void CreateMethod_WithNullArgs_ThrowsException()
    {
        _methodService!.CreateMethod(null!);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void CreateMethod_WhenMethodAlreadyExistsInClass_ShouldThrowException()
    {
        _methodRepositoryMock!
            .Setup(repo => repo.Exists(It.IsAny<Expression<Func<Method, bool>>>()))
            .Returns(true);

        _methodService!.CreateMethod(_testCreateMethodArgs);
    }

    #endregion

    #region Success

    [TestMethod]
    public void CreateMethod_WhenValid_ReturnsNewMethod()
    {
        _methodRepositoryMock!.Setup(repo => repo.Exists(It.IsAny<Expression<Func<Method, bool>>>())).Returns(false);
        _methodRepositoryMock.Setup(repo => repo.Add(It.IsAny<Method>())).Returns((Method act) => act);

        var result = _methodService!.CreateMethod(_testCreateMethodArgs);

        result.Should().NotBeNull();
        _methodRepositoryMock.VerifyAll();
    }

    #endregion

    #endregion

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
}
