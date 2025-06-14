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

    #endregion
}
