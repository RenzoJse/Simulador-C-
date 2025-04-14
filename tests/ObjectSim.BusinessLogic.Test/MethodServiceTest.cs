using ObjectSim.IDataAccess;
using Moq;
using ObjectSim.Domain;

namespace ObjectSim.BusinessLogic.Test;

[TestClass]
public class MethodServiceTest
{
    private Mock<IMethodRepository<Method>>? _methodRepositoryMock;
    private MethodService?  _methodService;

    [TestInitialize]
    public void Initialize()
    {
        _methodRepositoryMock = new Mock<IMethodRepository<Method>>(MockBehavior.Strict);
        _methodService = new MethodService(_methodRepositoryMock.Object);
    }
}
