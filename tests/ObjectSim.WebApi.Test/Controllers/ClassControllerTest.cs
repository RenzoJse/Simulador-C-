using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;
using ObjectSim.WebApi.Controllers;
using ObjectSim.WebApi.DTOs.In;
using ObjectSim.WebApi.DTOs.Out;

namespace ObjectSim.WebApi.Test.Controllers;

[TestClass]
public class ClassControllerTest
{
    private Mock<IClassService> _classServiceMock = null!;
    private ClassController _classController = null!;

    private readonly Class _testClass = new Class
    {
        Name = "TestClass",
        IsAbstract = false,
        IsInterface = false,
        IsSealed = false,
        Attributes = [],
        Methods = [],
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
        SetupHttpContext();
        _classServiceMock = new Mock<IClassService>();
        _classController = new ClassController(_classServiceMock.Object);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _classServiceMock.VerifyAll();
    }

    #region CreateClass

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

        var answer = resultObject?.Value as CreateClassDtoOut;
        answer.Should().NotBeNull();
        answer?.Name.Should().Be(_testClass.Name);
        answer?.IsAbstract.Should().Be(_testClass.IsAbstract);
    }

    #endregion

}
