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
    #endregion
}
