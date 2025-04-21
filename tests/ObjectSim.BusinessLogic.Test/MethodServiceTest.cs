using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain;

namespace ObjectSim.BusinessLogic.Test;

[TestClass]
public class MethodServiceTest
{
    private Mock<IRepository<Method>>? _methodRepositoryMock;
    private MethodService? _methodService;
    private static readonly Guid ClassId = Guid.NewGuid();
    private Method? testMethod;

    [TestInitialize]
    public void Initialize()
    {
        _methodRepositoryMock = new Mock<IRepository<Method>>(MockBehavior.Strict);
        _methodService = new MethodService(_methodRepositoryMock.Object);
        testMethod = new Method
        {
            Id = Guid.NewGuid(),
            Name = "MetodoDePrueba",
            Type = Method.MethodDataType.String,
            Abstract = false,
            IsSealed = false,
            Accessibility = Method.MethodAccessibility.Public,
            Parameters = [],
            LocalVariables = []
        };
    }

    [TestMethod]
    public void CreateMethod_WhenValid_ShouldReturnMethod()
    {
        _methodRepositoryMock!.Setup(repo => repo.Exists(It.IsAny<Expression<Func<Method, bool>>>())).Returns(false);
        _methodRepositoryMock.Setup(repo => repo.Add(It.IsAny<Method>())).Returns((Method act) => act);

        var result = _methodService!.Create(testMethod!);

        result.Should().NotBeNull();
        _methodRepositoryMock.VerifyAll();
    }

    [TestMethod]
    public void GetAllMethods_ShouldReturnMethods()
    {
        var methods = new List<Method>
        {
            new Method { Name = "m1Test", Abstract = false, IsSealed = false, Accessibility = Method.MethodAccessibility.Public, LocalVariables = [], Parameters = [] },
            new Method { Name = "m2Test", Abstract = false, IsSealed = false, Accessibility = Method.MethodAccessibility.Public, LocalVariables = [], Parameters = [] }
        };

        _methodRepositoryMock.Setup(repo => repo.GetAll(It.IsAny<Func<Method, bool>>())).Returns(methods);
        var result = _methodService.GetAll();

        result.Should().HaveCount(2);
        _methodRepositoryMock.Verify(repo => repo.GetAll(It.IsAny<Func<Method, bool>>()), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void GetAllMethods_ShouldThrowException_WhenRepositoryFails()
    {
        _methodRepositoryMock!.Setup(repo => repo.GetAll(It.IsAny<Func<Method, bool>>())).Throws(new Exception());

        _methodService!.GetAll();

        _methodRepositoryMock.VerifyAll();
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void DeleteMethod_WhenMethodNotFound_ShouldThrowException()
    {
        _methodRepositoryMock!.Setup(x => x.Get(It.IsAny<Func<Method, bool>>())).Returns((Method)null!);
        _methodService!.Delete(ClassId);

        _methodRepositoryMock.Verify(x => x.Get(It.IsAny<Func<Method, bool>>()), Times.Once);
        _methodRepositoryMock.Verify(x => x.Delete(It.IsAny<Method>()), Times.Never);
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

        _methodRepositoryMock!.Setup(x => x.Get(It.Is<Func<Method, bool>>(filter => filter(testMethod!)))).Returns(testMethod);
        _methodRepositoryMock!.Setup(x => x.Update(It.IsAny<Method>())).Returns((Method m) => m);

        var result = _methodService!.Update(testMethod.Id, newMethod);

        result.Should().NotBeNull();
        result.Id.Should().Be(testMethod.Id);
        result.Name.Should().Be(newMethod.Name);
        result.Type.Should().Be(newMethod.Type);
        result.Abstract.Should().Be(newMethod.Abstract);
        result.IsSealed.Should().Be(newMethod.IsSealed);
        result.Accessibility.Should().Be(newMethod.Accessibility);

        _methodRepositoryMock.Verify(x => x.Get(It.IsAny<Func<Method, bool>>()), Times.Once);
        _methodRepositoryMock.Verify(x => x.Update(It.IsAny<Method>()), Times.Once);
    }
}
