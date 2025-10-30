using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ObjectSim.IBusinessLogic;
using ObjectSim.WebApi.Controllers;
using ObjectSim.WebApi.DTOs.Out;

namespace ObjectSim.WebApi.Test.Controllers;

[TestClass]
public class OutputModelControllerTest
{
    private Mock<IOutputModelTransformerService> _outputModelTransformerService = null!;
    private OutputModelController _controllerTest = null!;

    [TestInitialize]
    public void Setup()
    {
        _outputModelTransformerService = new Mock<IOutputModelTransformerService>();
        _controllerTest = new OutputModelController(_outputModelTransformerService.Object);
    }
    #region Post

    [TestMethod]
    public void UploadDll_ValidDllFile_ReturnsOk()
    {
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.FileName).Returns("test.dll");
        fileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream());

        var result = _controllerTest.UploadDll(fileMock.Object);

        _outputModelTransformerService.Verify(s => s.UploadDll(It.IsAny<Stream>(), "test.dll"), Times.Once);
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public void UploadDll_ServiceThrowsException_ShouldPropagateException()
    {
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.FileName).Returns("bad.dll");
        fileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream());
        _outputModelTransformerService
            .Setup(s => s.UploadDll(It.IsAny<Stream>(), "bad.dll"))
            .Throws(new InvalidOperationException("upload fail"));

        Action act = () => _controllerTest.UploadDll(fileMock.Object);
        act.Should().Throw<InvalidOperationException>()
           .WithMessage("upload fail");
    }

    [TestMethod]
    public void UploadDll_ValidDllFile_ReturnsSuccessMessage()
    {
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.FileName).Returns("test.dll");
        fileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream());

        var result = _controllerTest.UploadDll(fileMock.Object) as OkObjectResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(200, result.StatusCode);
        Assert.AreEqual("DLL uploaded successfully.", result.Value);
    }
    #endregion

    #region Get

    [TestMethod]
    public void GetImplementationList_ReturnsOkWithList()
    {
        var implementations = new List<string> { "Impl1", "Impl2" };

        _outputModelTransformerService
            .Setup(s => s.GetImplementationList())
            .Returns(implementations);

        var result = _controllerTest.GetImplementationList();

        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.IsInstanceOfType(okResult.Value, typeof(List<OutputModelNameDtoOut>));
        var responseList = okResult.Value as List<OutputModelNameDtoOut>;
        Assert.AreEqual(2, responseList!.Count);
        Assert.IsTrue(responseList.Any(x => x.Name == "Impl1"));
        Assert.IsTrue(responseList.Any(x => x.Name == "Impl2"));
        _outputModelTransformerService.Verify(s => s.GetImplementationList(), Times.Once);
    }

    [TestMethod]
    public void GetImplementationList_WhenNone_ReturnsEmptyDtoList()
    {
        _outputModelTransformerService
            .Setup(s => s.GetImplementationList())
            .Returns([]);

        var ok = _controllerTest.GetImplementationList() as OkObjectResult;

        Assert.IsNotNull(ok);
        var list = ok.Value as List<OutputModelNameDtoOut>;
        Assert.IsNotNull(list);
        Assert.AreEqual(0, list.Count);
    }

    [TestMethod]
    public void GetImplementationList_ServiceThrowsException_ShouldPropagateException()
    {
        _outputModelTransformerService
            .Setup(s => s.GetImplementationList())
            .Throws(new Exception("fail get list"));

        Action act = () => _controllerTest.GetImplementationList();
        act.Should().Throw<Exception>()
           .WithMessage("fail get list");
    }

    [TestMethod]
    public void GetImplementationList_ShouldMapEachNameToDto()
    {
        var impls = new List<string> { "A", "B", "C" };
        _outputModelTransformerService
            .Setup(s => s.GetImplementationList())
            .Returns(impls);

        var ok = _controllerTest.GetImplementationList() as OkObjectResult;
        var dtos = ok!.Value as List<OutputModelNameDtoOut>;

        Assert.AreEqual(3, dtos!.Count);
        CollectionAssert.AreEqual(impls, dtos.Select(d => d.Name).ToList());
    }
    #endregion
}
