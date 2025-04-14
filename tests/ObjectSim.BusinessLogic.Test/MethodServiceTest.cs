using ObjectSim.IDataAccess;
using Moq;
using ObjectSim.Domain;
using ObjectSim.BusinessLogic;

namespace ObjectSim.BusinessLogic.Test;

[TestClass]
public class MethodServiceTest
{
    private Mock<IMethopdRepository<Method>>? _methodRepositoryMock;
    private MethodService?  _methodService;

    [TestInitialize]
    public void Initialize()
    {
        _methodRepositoryMock = new Mock<IMethopdRepository<Method>>(MockBehavior.Strict);
        _methodService = new MethodService(_methodRepositoryMock.Object);
    }
}
