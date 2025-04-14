using ObjectSim.IDataAccess;
using Moq;
using ObjectSim.Domain;
using System.Linq.Expressions;
using FluentAssertions;

namespace ObjectSim.BusinessLogic.Test;

[TestClass]
public class MethodServiceTest
{
    private Mock<IMethodRepository<Method>>? _methodRepositoryMock;
    private MethodService?  _methodService;

    [TestInitialize]
    public void Initialize()
    {
        _methodRepositoryMock = new Mock<IMethodRepository<Method>>(MockBehavior.Strict);
        _methodService = new MethodService(_methodRepositoryMock.Object);
    }


    [TestMethod]
    public void CreateValidMethod()
    {
        var testMethod = new Method
        {
            Id = 1,
            Name = "MetodoDePrueba",
            Type = "boolean",
            Abstract = false,
            IsSealed = false,
            Accessibility = "public",
            Parameters = new List<Parameter>(),
            LocalVariables = new List<LocalVariable>()
        };

        _methodRepositoryMock.Setup(repo => repo.Exist(It.IsAny<Expression<Func<Method, bool>>>())).Returns(false);
        _methodRepositoryMock.Setup(repo => repo.Add(It.IsAny<Method>())).Returns((Method act) => act);

        var result = _methodService.Create(testMethod);

        result.Should().NotBeNull();
        _methodRepositoryMock.VerifyAll();
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void CreateInvalidNameMethodData()
    {
        var testMethod = new Method
        {
            Id = 1,
            Name = string.Empty,
            Type = "boolean",
            Abstract = false,
            IsSealed = false,
            Accessibility = "public",
            Parameters = new List<Parameter>(),
            LocalVariables = new List<LocalVariable>()
        };

        _methodRepositoryMock.Setup(repo => repo.Exist(It.IsAny<Expression<Func<Method, bool>>>())).Returns(false);
        _methodRepositoryMock.Setup(repo => repo.Add(It.IsAny<Method>())).Returns((Method act) => act);

        var result = _methodService.Create(testMethod);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void CreateNullMethod()
    {
        _methodService.Create(null);
    }

    [TestMethod]
    public void GetAllTest()
    {
        var methods = new List<Method>
        {
            new Method{Id = 1, Name = "m1Test", Abstract = false, IsSealed =  false, Accessibility = "public", LocalVariables = new List<LocalVariable>(), Parameters = new List<Parameter>() },
            new Method{Id = 2, Name = "m1Test2", Abstract = false, IsSealed =  false, Accessibility = "public", LocalVariables = new List<LocalVariable>(), Parameters = new List<Parameter>() }
        };

        Assert.IsNotNull(_methodRepositoryMock);
        Assert.IsNotNull(_methodService);
        _methodRepositoryMock.Setup(repo => repo.GetAll()).Returns(methods);
        var result = _methodService.GetAll();
        result.Should().HaveCount(2);
        _methodRepositoryMock.Verify(repo => repo.GetAll(), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void GetAllThrowsBusinessDataException()
    {
        _methodRepositoryMock
            .Setup(repo => repo.GetAll())
            .Throws(new Exception());
        _methodService.GetAll();
        _methodRepositoryMock.VerifyAll();
    }

    [TestMethod]
    public void GetMethodById()
    {
        var testMethod = new Method
        {
            Id = 1,
            Name = "M1Test",
            Type = "boolean",
            Abstract = false,
            IsSealed = false,
            Accessibility = "public",
            Parameters = new List<Parameter>(),
            LocalVariables = new List<LocalVariable>()
        };

        _methodRepositoryMock.Setup(x => x.GetById(1)).Returns(testMethod);

        var result = _methodService.GetById(1);

        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Id);
        Assert.AreEqual("M1Test", result.Name);

        _methodRepositoryMock.Verify(x => x.GetById(1), Times.Once);
    }
}
