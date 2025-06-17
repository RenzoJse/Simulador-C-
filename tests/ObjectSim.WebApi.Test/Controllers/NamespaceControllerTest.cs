using FluentAssertions;
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

    #region GetAll
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
    public void GetAll_WhenNoNamespacesExist_ReturnsEmptyList()
    {
        _namespaceServiceMock
            .Setup(s => s.GetAll())
            .Returns(new List<Namespace>());

        var result = _controller.GetAll() as OkObjectResult;
        Assert.IsNotNull(result);
        Assert.AreEqual(200, result.StatusCode);

        var list = result.Value as List<NamespaceInformationDtoOut>;
        Assert.IsNotNull(list);
        Assert.AreEqual(0, list.Count);
    }

    [TestMethod]
    public void GetAll_ServiceThrowsException_ShouldPropagateException()
    {
        _namespaceServiceMock
            .Setup(s => s.GetAll())
            .Throws(new InvalidOperationException("error interno"));

        Action act = () => _controller.GetAll();
        act.Should().Throw<InvalidOperationException>()
           .WithMessage("error interno");
    }


    [TestMethod]
    public void GetAll_ShouldMapParentIdCorrectly()
    {
        var parentId = Guid.NewGuid();
        var entities = new List<Namespace>
    {
        new Namespace { Id = Guid.NewGuid(), Name = "A", ParentId = parentId }
    };
        _namespaceServiceMock.Setup(s => s.GetAll()).Returns(entities);

        var ok = _controller.GetAll() as OkObjectResult;
        var dto = (ok!.Value as List<NamespaceInformationDtoOut>)!.First();

        Assert.AreEqual(parentId, dto.ParentId);
    }
    #endregion

    #region Create 
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
    public void Create_NullDto_ShouldThrowNullReferenceException()
    {
        Action act = () => _controller.Create(null!);
        act.Should().Throw<NullReferenceException>();
    }

    [TestMethod]
    public void Create_ServiceThrowsException_ShouldPropagateException()
    {
        var dto = new CreateNamespaceDtoIn { Name = "N1", ParentId = null };
        _namespaceServiceMock
            .Setup(s => s.Create(It.IsAny<CreateNamespaceArgs>()))
            .Throws(new InvalidOperationException("Created failed"));

        Action act = () => _controller.Create(dto);
        act.Should().Throw<InvalidOperationException>()
           .WithMessage("Created failed");
    }

    [TestMethod]
    public void Create_WithParentId_CallsServiceWithCorrectArgs()
    {
        var parentId = Guid.NewGuid();
        var dto = new CreateNamespaceDtoIn { Name = "N1", ParentId = parentId };
        _namespaceServiceMock
            .Setup(s => s.Create(It.Is<CreateNamespaceArgs>(a =>
                a.Name == "N1" && a.ParentId == parentId
            )))
            .Returns(new Namespace());

        var ok = _controller.Create(dto) as OkResult;
        Assert.AreEqual(200, ok!.StatusCode);
    }
    #endregion

    #region GetAllDescendants
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

    [TestMethod]
    public void GetAllDescendants_ServiceThrowsException_ShouldPropagateException()
    {
        var id = Guid.NewGuid();
        _namespaceServiceMock
            .Setup(s => s.GetAllDescendants(id))
            .Throws(new InvalidOperationException("error descendants"));

        Action act = () => _controller.GetAllDescendants(id);
        act.Should().Throw<InvalidOperationException>()
           .WithMessage("error descendants");
    }

    [TestMethod]
    public void GetAllDescendants_EmptyGuid_ShouldThrowArgumentException()
    {
        _namespaceServiceMock
            .Setup(s => s.GetAllDescendants(Guid.Empty))
            .Throws(new ArgumentException("Invalid id"));

        Action act = () => _controller.GetAllDescendants(Guid.Empty);
        act.Should().Throw<ArgumentException>()
           .WithMessage("Invalid id");
    }
    #endregion

}
