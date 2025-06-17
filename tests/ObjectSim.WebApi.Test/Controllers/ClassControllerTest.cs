using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;
using ObjectSim.WebApi.Controllers;
using ObjectSim.WebApi.DTOs.In;
using ObjectSim.WebApi.DTOs.Out;
using Attribute = ObjectSim.Domain.Attribute;

namespace ObjectSim.WebApi.Test.Controllers;

[TestClass]
public class ClassControllerTest
{
    private Mock<IClassService> _classServiceMock = null!;
    private ClassController _classController = null!;

    private static readonly Attribute TestAttribute = new() { Name = "TestAttribute", };

    private static readonly Method TestMethod = new() { Name = "TestMethod", };

    private readonly Class _testClass = new()
    {
        Name = "TestClass",
        IsAbstract = false,
        IsInterface = false,
        IsSealed = false,
        Attributes = [TestAttribute],
        Methods = [TestMethod],
        Parent = null,
    };

    private void SetupHttpContext()
    {
        var context = new DefaultHttpContext();
        _classController.ControllerContext.HttpContext = context;
    }

    [TestInitialize]
    public void Setup()
    {
        _classServiceMock = new Mock<IClassService>();
        _classController = new ClassController(_classServiceMock.Object);
        SetupHttpContext();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _classServiceMock.VerifyAll();
    }

    #region CreateClass-POST

    [TestMethod]
    public void CreateClass_WhenIsValid_MakesValidPost()
    {
        _classServiceMock
            .Setup(service => service.CreateClass(It.IsAny<CreateClassArgs>()))
            .Returns(_testClass);

        var result = _classController.CreateClass(new CreateClassDtoIn
        {
            Name = "TestClass",
            IsAbstract = false,
            IsInterface = false,
            IsSealed = false,
            Attributes = [],
            Methods = [],
            Parent = null,
        });

        var resultObject = result as OkObjectResult;
        var statusCode = resultObject?.StatusCode;
        statusCode.Should().Be(200);

        var answer = resultObject?.Value as ClassInformationDtoOut;
        answer.Should().NotBeNull();
        answer.Name.Should().Be(_testClass.Name);
        answer.IsAbstract.Should().Be((bool)_testClass.IsAbstract!);
        answer.IsInterface.Should().Be((bool)_testClass.IsInterface!);
        answer.IsSealed.Should().Be((bool)_testClass.IsSealed!);
        answer.Attributes.Should().BeEquivalentTo(_testClass.Attributes!.Select(attribute => attribute.Name));
        answer.Methods.Should().BeEquivalentTo(_testClass.Methods!.Select(method => method.Name));
        answer.Parent.Should().Be(_testClass.Parent?.Id);
        answer.Id.Should().Be(_testClass.Id);
    }

    [TestMethod]
    public void CreateClass_NullDto_ShouldThrowNullReferenceException()
    {
        Action act = () => _classController.CreateClass(null!);
        act.Should().Throw<NullReferenceException>();
    }

    [TestMethod]
    public void CreateClass_ServiceThrowsException_ShouldPropagateException()
    {
        var dtoIn = new CreateClassDtoIn { Name = "T1", IsAbstract = false, IsInterface = false, IsSealed = false };
        _classServiceMock
            .Setup(s => s.CreateClass(It.IsAny<CreateClassArgs>()))
            .Throws(new InvalidOperationException("internal error"));

        Action act = () => _classController.CreateClass(dtoIn);
        act.Should().Throw<InvalidOperationException>()
           .WithMessage("internal error");
    }
    #endregion

    #region GetClass-GET

    [TestMethod]
    public void GetClass_WhenIsValid_MakesValidGet()
    {
        var classId = Guid.NewGuid();
        _classServiceMock
            .Setup(service => service.GetById(classId))
            .Returns(_testClass);

        var result = _classController.GetClass(classId);

        var resultObject = result as OkObjectResult;
        var statusCode = resultObject?.StatusCode;
        statusCode.Should().Be(200);

        var answer = resultObject?.Value as ClassInformationDtoOut;
        answer.Should().NotBeNull();
        answer.Name.Should().Be(_testClass.Name);
        answer.IsAbstract.Should().Be((bool)_testClass.IsAbstract!);
        answer.IsInterface.Should().Be((bool)_testClass.IsInterface!);
        answer.IsSealed.Should().Be((bool)_testClass.IsSealed!);
        answer.Attributes.Should().BeEquivalentTo(_testClass.Attributes!.Select(attribute => attribute.Name));
        answer.Methods.Should().BeEquivalentTo(_testClass.Methods!.Select(method => method.Name));
        answer.Parent.Should().Be(_testClass.Parent?.Id);
        answer.Id.Should().Be(_testClass.Id);
    }

    [TestMethod]
    public void GetClass_ServiceThrowsException_ShouldPropagateException()
    {
        var id = Guid.NewGuid();
        _classServiceMock
            .Setup(s => s.GetById(id))
            .Throws(new KeyNotFoundException("no existe"));

        Action act = () => _classController.GetClass(id);
        act.Should().Throw<KeyNotFoundException>()
           .WithMessage("no existe");
    }

    [TestMethod]
    public void GetClass_EmptyGuid_ShouldThrowArgumentException()
    {
        var empty = Guid.Empty;
        _classServiceMock
            .Setup(s => s.GetById(empty))
            .Throws(new ArgumentException("Id inválido"));

        Action act = () => _classController.GetClass(empty);
        act.Should().Throw<ArgumentException>()
           .WithMessage("Id inválido");
    }
    #endregion

    #region DeleteClass_DELETE

    [TestMethod]
    public void DeleteClass_WhenIsValid_MakesValidDelete()
    {
        var classId = Guid.NewGuid();
        _classServiceMock
            .Setup(service => service.DeleteClass(classId));

        var result = _classController.DeleteClass(classId);

        var resultObject = result as OkResult;
        var statusCode = resultObject?.StatusCode;
        statusCode.Should().Be(200);
    }

    [TestMethod]
    public void DeleteClass_ServiceThrowsException_ShouldPropagateException()
    {
        var id = Guid.NewGuid();
        _classServiceMock
            .Setup(s => s.DeleteClass(id))
            .Throws(new InvalidOperationException("fail delete"));

        Action act = () => _classController.DeleteClass(id);
        act.Should().Throw<InvalidOperationException>()
           .WithMessage("fail delete");
    }

    [TestMethod]
    public void DeleteClass_EmptyGuid_ShouldThrowArgumentException()
    {
        _classServiceMock
            .Setup(s => s.DeleteClass(Guid.Empty))
            .Throws(new ArgumentException("Empty id"));

        Action act = () => _classController.DeleteClass(Guid.Empty);
        act.Should().Throw<ArgumentException>()
           .WithMessage("Empty id");
    }

    #endregion

    #region RemoveMethod-PATCH

    [TestMethod]
    public void RemoveMethod_WhenIsValid_MakesValidPatch()
    {
        var methodId = Guid.NewGuid();

        _classServiceMock
            .Setup(service => service.RemoveMethod(_testClass.Id, methodId));

        var result = _classController.RemoveMethod(_testClass.Id, methodId);

        var resultObject = result as OkResult;
        var statusCode = resultObject?.StatusCode;
        statusCode.Should().Be(200);
    }

    [TestMethod]
    public void RemoveMethod_ServiceThrowsException_ShouldPropagateException()
    {
        var cId = _testClass.Id;
        var mId = Guid.NewGuid();
        _classServiceMock
            .Setup(s => s.RemoveMethod(cId, mId))
            .Throws(new InvalidOperationException("Cant remove method"));

        Action act = () => _classController.RemoveMethod(cId, mId);
        act.Should().Throw<InvalidOperationException>()
           .WithMessage("Cant remove method");
    }

    #endregion

    #region RemoveAttribute-PATCH

    [TestMethod]
    public void RemoveAttribute_WhenIsValid_MakesValidPatch()
    {
        var attributeId = Guid.NewGuid();

        _classServiceMock
            .Setup(service => service.RemoveAttribute(_testClass.Id, attributeId));

        var result = _classController.RemoveAttribute(_testClass.Id, attributeId);

        var resultObject = result as OkResult;
        var statusCode = resultObject?.StatusCode;
        statusCode.Should().Be(200);
    }

    #endregion

    #region GetAll-Classes-Test
    [TestMethod]
    public void GetAllMethods_ShouldReturnAllMethods()
    {
        var classes = new List<Class> { _testClass };

        _classServiceMock
            .Setup(service => service.GetAll())
            .Returns(classes);

        var result = _classController.GetAllClasses();

        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);

        var response = okResult.Value as List<ClassDtoOut>;
        response.Should().NotBeNull();
        response!.Count.Should().Be(classes.Count);
        response.First().Name.Should().Be(_testClass.Name);
    }

    [TestMethod]
    public void GetAllClasses_WhenNoClassesExist_ShouldReturnEmptyList()
    {
        _classServiceMock
            .Setup(s => s.GetAll())
            .Returns([]);

        var ok = _classController.GetAllClasses() as OkObjectResult;
        var list = ok!.Value as List<ClassDtoOut>;

        ok.StatusCode.Should().Be(200);
        list.Should().BeEmpty();
    }

    [TestMethod]
    public void GetAllClasses_ServiceThrowsException_ShouldPropagateException()
    {
        _classServiceMock
            .Setup(s => s.GetAll())
            .Throws(new Exception("fail get all"));

        Action act = () => _classController.GetAllClasses();
        act.Should().Throw<Exception>()
           .WithMessage("fail get all");
    }
    #endregion

    #region Update-Class-Test
    [TestMethod]
    public void UpdateClass_WhenIsValid_MakesValidUpdate()
    {
        var classId = Guid.NewGuid();
        var dto = new UpdateClassNameDto { Name = "UpdatedName" };

        _classServiceMock
            .Setup(service => service.UpdateClass(classId, dto.Name));

        var result = _classController.UpdateClass(classId, dto);

        var resultObject = result as OkResult;
        var statusCode = resultObject?.StatusCode;
        statusCode.Should().Be(200);

        _classServiceMock.Verify(service => service.UpdateClass(classId, dto.Name), Times.Once);
    }

    [TestMethod]
    public void UpdateClass_NullDto_ShouldThrowNullReferenceException()
    {
        Action act = () => _classController.UpdateClass(Guid.NewGuid(), null!);
        act.Should().Throw<NullReferenceException>();
    }

    [TestMethod]
    public void UpdateClass_ServiceThrowsException_ShouldPropagateException()
    {
        var id = Guid.NewGuid();
        var dto = new UpdateClassNameDto { Name = "X" };
        _classServiceMock
            .Setup(s => s.UpdateClass(id, dto.Name))
            .Throws(new KeyNotFoundException("Not found"));

        Action act = () => _classController.UpdateClass(id, dto);
        act.Should().Throw<KeyNotFoundException>()
           .WithMessage("Not found");
    }

    [TestMethod]
    public void UpdateClass_EmptyGuid_ShouldThrowArgumentException()
    {
        var dto = new UpdateClassNameDto { Name = "C1" };
        _classServiceMock
            .Setup(s => s.UpdateClass(Guid.Empty, dto.Name))
            .Throws(new ArgumentException("Id inválido"));

        Action act = () => _classController.UpdateClass(Guid.Empty, dto);
        act.Should().Throw<ArgumentException>()
           .WithMessage("Id inválido");
    }
    #endregion
}

