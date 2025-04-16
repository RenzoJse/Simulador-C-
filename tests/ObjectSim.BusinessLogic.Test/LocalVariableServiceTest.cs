using Moq;
using ObjectSim.Domain;
using ObjectSim.IDataAccess;

namespace ObjectSim.BusinessLogic.Test;
[TestClass]
public class ParameterServiceTest
{
    private Mock<ILocalVariableRepository<LocalVariable>>? _localVariableRepositoryMock;
    private LocalVariableService? _localVariableService;
    private static readonly Guid ClassId = Guid.NewGuid();
    private LocalVariable? _localVariable;


    [TestInitialize]
    public void Setup()
    {
        _localVariableRepositoryMock = new Mock<ILocalVariableRepository<LocalVariable>>(MockBehavior.Strict);
        _localVariableService = new LocalVariableService(_localVariableRepositoryMock.Object);
        _localVariable = new LocalVariable
        {
            Id = ClassId,
            Name = "Parameter1",
            Type = "string"
        };
    }
}
