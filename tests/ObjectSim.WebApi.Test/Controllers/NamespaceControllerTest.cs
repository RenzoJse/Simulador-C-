

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

        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var returnedDtos = okResult.Value as List<NamespaceInformationDtoOut>;
        Assert.IsNotNull(returnedDtos);
        Assert.AreEqual(namespaces.Count, returnedDtos.Count);
    }

    [TestMethod]
    public void Create_WithValidDto_ReturnsOkStatus()
    {
        var dtoIn = new CreateNamespaceDtoIn
        {
            Name = "MyNS",
            ParentId = null
        };

        _namespaceServiceMock
            .Setup(s => s.Create(It.IsAny<CreateNamespaceArgs>()))
            .Returns(new Namespace { Id = Guid.NewGuid(), Name = "MyNS" });

        var result = _controller.Create(dtoIn);

        var okResult = result as OkResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
    }

    [TestMethod]
    public void GetAllDescendants_WithValidNamespaceId_ReturnsDescendantDtos()
    {
        var parentId = Guid.NewGuid();
        var descendants = new List<Namespace>
    {
        new Namespace { Id = Guid.NewGuid(), Name = "Child1", ParentId = parentId },
        new Namespace { Id = Guid.NewGuid(), Name = "Child2", ParentId = parentId }
    };

        _namespaceServiceMock
            .Setup(s => s.GetAllDescendants(parentId))
            .Returns(descendants);

        var result = _controller.GetAllDescendants(parentId);

        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var returnedDtos = okResult.Value as List<NamespaceInformationDtoOut>;
        Assert.IsNotNull(returnedDtos);
        Assert.AreEqual(descendants.Count, returnedDtos.Count);
        Assert.IsTrue(returnedDtos.Any(dto => dto.Name == "Child1"));
        Assert.IsTrue(returnedDtos.Any(dto => dto.Name == "Child2"));
    }
    [TestMethod]
    public void GetAllDescendants_WhenNoneFound_ReturnsEmptyList()
    {
        var parentId = Guid.NewGuid();

        _namespaceServiceMock
            .Setup(s => s.GetAllDescendants(parentId))
            .Returns([]);

        var result = _controller.GetAllDescendants(parentId);

        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var returnedDtos = okResult.Value as List<NamespaceInformationDtoOut>;
        Assert.IsNotNull(returnedDtos);
        Assert.AreEqual(0, returnedDtos.Count);
    }


}
