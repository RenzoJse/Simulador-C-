
using Moq;
using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain.Args;
using ObjectSim.Domain;

namespace ObjectSim.BusinessLogic.Test;
[TestClass]
public class NamespaceServiceTest
{
    private Mock<IRepository<Namespace>> _namespaceRepositoryMock = null!;
    private NamespaceService _namespaceService = null!;
    [TestInitialize]
    public void Setup()
    {
        _namespaceRepositoryMock = new Mock<IRepository<Namespace>>();
        _namespaceService = new NamespaceService(_namespaceRepositoryMock.Object);
    }
    [TestMethod]
    public void Create_WithValidArgs_CallsAddOnRepository()
    {
        var args = new CreateNamespaceArgs("MyNamespace", null);

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
        var args = new CreateNamespaceArgs("ChildNamespace", parentId);

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
    public void GetAll_ShouldReturnAllNamespaces()
    {
        var namespaces = new List<Namespace>
        {
            new Namespace { Id = Guid.NewGuid(), Name = "System" },
            new Namespace { Id = Guid.NewGuid(), Name = "App" }
        };

        _namespaceRepositoryMock
            .Setup(r => r.GetAll(It.IsAny<Func<Namespace, bool>>()))
            .Returns(namespaces);

        var result = _namespaceService.GetAll();

        CollectionAssert.AreEqual(namespaces, result);
    }
}
