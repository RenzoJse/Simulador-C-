using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using ObjectSim.Domain;
using ObjectSim.IDataAccess;

namespace ObjectSim.BusinessLogic.Test;

[TestClass]
public class MethodServiceTest
{
    private Mock<IMethodRepository<Method>>? _methodRepositoryMock;
    private MethodService? _methodService;
    private static readonly Guid ClassId = Guid.NewGuid();

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
            Id = Guid.NewGuid(),
            Name = "MetodoDePrueba",
            Type = "boolean",
            Abstract = false,
            IsSealed = false,
            Accessibility = "public",
            Parameters = [],
            LocalVariables = []
        };

        _methodRepositoryMock!.Setup(repo => repo.Exist(It.IsAny<Expression<Func<Method, bool>>>())).Returns(false);
        _methodRepositoryMock.Setup(repo => repo.Add(It.IsAny<Method>())).Returns((Method act) => act);

        var result = _methodService!.Create(testMethod);

        result.Should().NotBeNull();
        _methodRepositoryMock.VerifyAll();
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void CreateInvalidNameMethodData()
    {
        var testMethod = new Method
        {
            Name = string.Empty,
            Type = "boolean",
            Abstract = false,
            IsSealed = false,
            Accessibility = "public",
            Parameters = [],
            LocalVariables = []
        };

        _methodRepositoryMock!.Setup(repo => repo.Exist(It.IsAny<Expression<Func<Method, bool>>>())).Returns(false);
        _methodRepositoryMock.Setup(repo => repo.Add(It.IsAny<Method>())).Returns((Method act) => act);

        var result = _methodService!.Create(testMethod);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void CreateNullMethod()
    {
        _methodService!.Create(null!);
    }

    [TestMethod]
    public void GetAllTest()
    {
        var methods = new List<Method>
        {
            new Method{Name = "m1Test", Abstract = false, IsSealed =  false, Accessibility = "public", LocalVariables = [], Parameters = []},
            new Method{Name = "m1Test2", Abstract = false, IsSealed =  false, Accessibility = "public", LocalVariables = [], Parameters = []}
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
        _methodRepositoryMock!
            .Setup(repo => repo.GetAll())
            .Throws(new Exception());
        _methodService!.GetAll();
        _methodRepositoryMock.VerifyAll();
    }

    [TestMethod]
    public void GetMethodById()
    {
        var testMethod = new Method
        {
            Id = ClassId,
            Name = "M1Test",
            Type = "boolean",
            Abstract = false,
            IsSealed = false,
            Accessibility = "public",
            Parameters = [],
            LocalVariables = []
        };

        _methodRepositoryMock!.Setup(x => x.GetById(ClassId)).Returns(testMethod);

        var result = _methodService!.GetById(ClassId);

        Assert.IsNotNull(result);
        Assert.AreEqual(ClassId, result.Id);
        Assert.AreEqual("M1Test", result.Name);

        _methodRepositoryMock.Verify(x => x.GetById(ClassId), Times.Once);
    }

    [TestMethod]
    public void DeleteMethod()
    {
        var testMethod = new Method
        {
            Id = ClassId,
            Name = "M1Test",
            Type = "boolean",
            Abstract = false,
            IsSealed = false,
            Accessibility = "public",
            Parameters = [],
            LocalVariables = []
        };

        _methodRepositoryMock!.Setup(x => x.GetById(ClassId)).Returns(testMethod);
        _methodRepositoryMock.Setup(x => x.Remove(testMethod));

        _methodService!.Delete(testMethod.Id);

        _methodRepositoryMock.Verify(x => x.GetById(ClassId), Times.Once);
        _methodRepositoryMock.Verify(x => x.Remove(testMethod), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]

    public void DeleteNullMethod()
    {
        var testMethod = new Method
        {
            Id = ClassId,
            Name = "M1Test",
            Type = "boolean",
            Abstract = false,
            IsSealed = false,
            Accessibility = "public",
            Parameters = [],
            LocalVariables = []
        };

        _methodRepositoryMock!.Setup(x => x.GetById(ClassId)).Returns((Method)null!);

        _methodService!.Delete(ClassId);

        _methodRepositoryMock.Verify(x => x.Get(It.IsAny<Expression<Func<Method, bool>>>()), Times.Once);
        _methodRepositoryMock.Verify(x => x.Remove(It.IsAny<Method>()), Times.Never);
    }

    [TestMethod]
    public void UpdateUserTest()
    {
        var testMethod = new Method
        {
            Id = ClassId,
            Name = "M1Test",
            Type = "boolean",
            Abstract = false,
            IsSealed = false,
            Accessibility = "public",
            Parameters = [],
            LocalVariables = []
        };

        var newMethod = new Method
        {
            Id = ClassId,
            Name = "M1New",
            Type = "boolean",
            Abstract = false,
            IsSealed = false,
            Accessibility = "public",
            Parameters = [],
            LocalVariables = []
        };

        _methodRepositoryMock!.Setup(x => x.GetById(testMethod.Id)).Returns(testMethod);
        _methodRepositoryMock.Setup(x => x.Update(testMethod));

        _methodService!.Update(testMethod.Id, newMethod);

        _methodRepositoryMock.VerifyAll();

        Assert.AreEqual(testMethod.Name, newMethod.Name);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void UpdateInvalidMethodNameTest()
    {
        var testMethod = new Method
        {
            Id = ClassId,
            Name = "M1Test",
            Type = "boolean",
            Abstract = false,
            IsSealed = false,
            Accessibility = "public",
            Parameters = [],
            LocalVariables = []
        };

        Guid secondClassId = Guid.NewGuid();

        var newMethod = new Method
        {
            Id = secondClassId,
            Name = "",
            Type = "boolean",
            Abstract = false,
            IsSealed = false,
            Accessibility = "public",
            Parameters = [],
            LocalVariables = []
        };

        _methodRepositoryMock!.Setup(x => x.GetById(testMethod.Id)).Returns(testMethod);
        _methodRepositoryMock.Setup(x => x.Update(testMethod));

        _methodService!.Update(testMethod.Id, newMethod);

        _methodRepositoryMock.VerifyAll();

        Assert.AreEqual(testMethod.Name, newMethod.Name);
    }
}
