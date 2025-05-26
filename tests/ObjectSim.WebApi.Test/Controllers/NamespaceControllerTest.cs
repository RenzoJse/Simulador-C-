

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
public class NamespaceControllerTest
{
    private Mock<INamespaceService> _namespaceServiceMock = null!;
    private NamespaceController _controller = null!;
    [TestInitialize]
    public void Setup()
    {
        _namespaceServiceMock = new Mock<INamespaceService>();
        _controller = new NamespaceController(_namespaceServiceMock.Object);
    }
    [TestMethod]
    public void GetAll_ShouldReturnOkWithNamespaceDtos()
    {
        var namespaces = new List<Namespace>
        {
            new Namespace { Id = Guid.NewGuid(), Name = "System" },
            new Namespace { Id = Guid.NewGuid(), Name = "App", ParentId = Guid.NewGuid() }
        };

        _namespaceServiceMock.Setup(s => s.GetAll()).Returns(namespaces);

        var result = _controller.GetAll();

        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var returnedDtos = okResult.Value as List<NamespaceInformationDtoOut>;
        Assert.IsNotNull(returnedDtos);
        Assert.AreEqual(namespaces.Count, returnedDtos.Count);
    }
    [TestMethod]
    public void Create_WithValidDto_ReturnsCreatedAtActionWithDto()
    {
        var dtoIn = new CreateNamespaceDtoIn
        {
            Name = "NewNamespace",
            ParentId = null
        };

        var created = new Namespace
        {
            Id = Guid.NewGuid(),
            Name = "NewNamespace",
            ParentId = null
        };

        _namespaceServiceMock
            .Setup(s => s.Create(It.Is<CreateNamespaceArgs>(a => a.Name == "NewNamespace")))
            .Returns(created);

        var result = _controller.Create(dtoIn);

        var createdResult = result.Result as CreatedAtActionResult;
        Assert.IsNotNull(createdResult);
        Assert.AreEqual("GetById", createdResult.ActionName);
        Assert.AreEqual(created.Id, ((NamespaceInformationDtoOut)createdResult.Value!).Id);
    }
}
