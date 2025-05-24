
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
}
