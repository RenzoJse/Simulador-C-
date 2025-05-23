
using Moq;
using ObjectSim.DataAccess.Interface;

namespace ObjectSim.BusinessLogic.Test;
[TestClass]
public class NamespaceServiceTest
{
    private NamespaceService _namespaceService;
    private Mock<IRepository<Namespace>> _namespaceRepositoryMock;
    [TestInitialize]
    public void Setup()
    {
        _namespaceRepositoryMock = new Mock<IRepository<Namespace>>();
        _namespaceService = new NamespaceService(_namespaceRepositoryMock.Object);
    }
    [TestMethod]
    public void Create_WithValidNamespace_CallsAddOnRepository()
    {
        // Arrange
        var ns = new Namespace { Name = "Utilities" };

        // Act
        _service.Create(ns);

        // Assert
        _namespaceRepoMock.Verify(r => r.Add(ns), Times.Once);
    }
}
