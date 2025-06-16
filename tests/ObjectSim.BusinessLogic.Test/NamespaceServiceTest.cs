using Moq;
using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.BusinessLogic.Test;

[TestClass]
public class NamespaceServiceTest
{
    private Mock<INamespaceRepository> _namespaceRepositoryMock = null!;
    private Mock<IClassService> _classServiceMock = null!;
    private NamespaceService _namespaceService = null!;

    [TestInitialize]
    public void Setup()
    {
        _namespaceRepositoryMock = new Mock<INamespaceRepository>();
        _classServiceMock = new Mock<IClassService>();
        _namespaceService = new NamespaceService(_namespaceRepositoryMock.Object, _classServiceMock.Object);
    }

    [TestMethod]
    public void Create_WithValidArgs_CallsAddOnRepository()
    {
        var args = new CreateNamespaceArgs("MyNamespace", null, []);

        _namespaceService.Create(args);

        _namespaceRepositoryMock.Verify(r =>
            r.Add(It.Is<Namespace>(n => n.Name == "MyNamespace" && n.ParentId == null)), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Create_WithNullArgs_ThrowsArgumentNullException()
    {
        _namespaceService.Create(null!);
    }

    [TestMethod]
    public void Create_WithParentId_SetsParentCorrectly()
    {
        var parentId = Guid.NewGuid();
        var args = new CreateNamespaceArgs("ChildNamespace", parentId, []);

        Namespace? capturedNamespace = null;

        _namespaceRepositoryMock
            .Setup(r => r.Add(It.IsAny<Namespace>()))
            .Callback<Namespace>(ns => capturedNamespace = ns)
            .Returns<Namespace>(ns => ns);

        var result = _namespaceService.Create(args);

        Assert.IsNotNull(capturedNamespace);
        Assert.AreEqual("ChildNamespace", capturedNamespace!.Name);
        Assert.AreEqual(parentId, capturedNamespace.ParentId);
    }

    [TestMethod]
    public void Create_ReturnsCreatedNamespace()
    {
        var args = new CreateNamespaceArgs("ReturnedNS", null, []);

        var expected = new Namespace
        {
            Id = args.Id,
            Name = args.Name,
            ParentId = args.ParentId
        };

        _namespaceRepositoryMock
            .Setup(r => r.Add(It.IsAny<Namespace>()))
            .Returns(expected);

        var result = _namespaceService.Create(args);

        Assert.AreEqual(expected.Id, result.Id);
        Assert.AreEqual(expected.Name, result.Name);
        Assert.AreEqual(expected.ParentId, result.ParentId);
    }

    [TestMethod]
    public void GetAll_ShouldReturnAllNamespaces()
    {
        var namespaces = new List<Namespace>
        {
            new Namespace { Id = Guid.NewGuid(), Name = "System" },
            new Namespace { Id = Guid.NewGuid(), Name = "App" }
        };

        _namespaceRepositoryMock
            .Setup(r => r.GetAll())
            .Returns(namespaces);

        var result = _namespaceService.GetAll();

        CollectionAssert.AreEqual(namespaces, result);
    }

    [TestMethod]
    public void GetAllDescendants_WithValidId_ReturnsDescendants()
    {
        var child = new Namespace { Id = Guid.NewGuid(), Name = "Child" };
        var parent = new Namespace
        {
            Id = Guid.NewGuid(),
            Name = "Parent",
            Children = [child]
        };

        _namespaceRepositoryMock
            .Setup(r => r.GetByIdWithChildren(parent.Id))
            .Returns(parent);

        var result = _namespaceService.GetAllDescendants(parent.Id);

        CollectionAssert.Contains(result.ToList(), child);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void GetAllDescendants_WithInvalidId_ThrowsArgumentException()
    {
        _namespaceRepositoryMock
            .Setup(r => r.GetByIdWithChildren(It.IsAny<Guid>()))
            .Returns((Namespace?)null);

        _namespaceService.GetAllDescendants(Guid.NewGuid());
    }
}
