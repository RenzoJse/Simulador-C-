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

    [TestInitialize]
    public void Initialize()
    {
        _methodRepositoryMock = new Mock<IRepository<Method>>(MockBehavior.Strict);
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

        _methodRepositoryMock!.Setup(repo => repo.Exists(It.IsAny<Expression<Func<Method, bool>>>())).Returns(false);
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

        _methodRepositoryMock!.Setup(repo => repo.Exists(It.IsAny<Expression<Func<Method, bool>>>())).Returns(false);
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
        _methodRepositoryMock.Setup(repo => repo.GetAll(It.IsAny<Func<Method, bool>>()))
            .Returns(methods);
        var result = _methodService.GetAll();
        result.Should().HaveCount(2);
        _methodRepositoryMock.Verify(repo => repo.GetAll(It.IsAny<Func<Method, bool>>()), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void GetAllThrowsBusinessDataException()
    {
        _methodRepositoryMock!
            .Setup(repo => repo.GetAll(It.IsAny<Func<Method, bool>>()))
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

        _methodRepositoryMock!.Setup(x => x.Get(It.Is<Func<Method, bool>>(filter => filter(testMethod))))
            .Returns(testMethod);

        var result = _methodService!.GetById(ClassId);

        Assert.IsNotNull(result);
        Assert.AreEqual(ClassId, result.Id);
        Assert.AreEqual("M1Test", result.Name);

        _methodRepositoryMock.Verify(x => x.Get(It.IsAny<Func<Method, bool>>()), Times.Once);
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

        _methodRepositoryMock!.Setup(x => x.Get(It.Is<Func<Method, bool>>(filter => filter(testMethod))))
            .Returns(testMethod);
        _methodRepositoryMock.Setup(x => x.Delete(It.Is<Method>(m => m.Id == ClassId)));

        _methodService!.Delete(ClassId);

        _methodRepositoryMock.Verify(x => x.Get(It.IsAny<Func<Method, bool>>()), Times.Once);
        _methodRepositoryMock.Verify(x => x.Delete(It.Is<Method>(m => m.Id == ClassId)), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void DeleteNullMethod()
    {
        _methodRepositoryMock!.Setup(x => x.Get(It.IsAny<Func<Method, bool>>()))
            .Returns((Method)null!);

        _methodService!.Delete(ClassId);

        _methodRepositoryMock.Verify(x => x.Get(It.IsAny<Func<Method, bool>>()), Times.Once);
        _methodRepositoryMock.Verify(x => x.Delete(It.IsAny<Method>()), Times.Never);
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
            Name = "M1Updated",
            Type = "string",
            Abstract = true,
            IsSealed = true,
            Accessibility = "private",
            Parameters = [],
            LocalVariables = []
        };

        _methodRepositoryMock!.Setup(x => x.Get(It.Is<Func<Method, bool>>(filter => filter(testMethod))))
            .Returns(testMethod);

        _methodRepositoryMock!.Setup(x => x.Update(It.IsAny<Method>()))
            .Returns((Method m) => m);

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

        var secondClassId = Guid.NewGuid();

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

        _methodRepositoryMock!.Setup(x => x.Get(It.Is<Func<Method, bool>>(filter => filter(testMethod))))
            .Returns(testMethod);

        _methodRepositoryMock.Setup(x => x.Update(It.IsAny<Method>()));

        _methodService!.Update(testMethod.Id, newMethod);

        _methodRepositoryMock.Verify(x => x.Get(It.IsAny<Func<Method, bool>>()), Times.Once);
        _methodRepositoryMock.Verify(x => x.Update(It.IsAny<Method>()), Times.Never);
    }
}
