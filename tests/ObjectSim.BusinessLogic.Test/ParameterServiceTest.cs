using Moq;
using ObjectSim.Domain;
using ObjectSim.IBusinessLogic;
using ObjectSim.IDataAccess;

namespace ObjectSim.BusinessLogic.Test;

[TestClass]
public class ParameterServiceTest
{
    private Mock<IParameterRepository<Parameter>>? _parameterRepositoryMock;
    private ParameterService? _parameterService;
    private static readonly Guid ClassId = Guid.NewGuid();
    private Parameter? parameter;


    [TestInitialize]
    public void Setup()
    {
        _parameterRepositoryMock = new Mock<IParameterRepository<Parameter>>(MockBehavior.Strict);
        _parameterService = new MethodService(_parameterRepositoryMock.Object);
        parameter = new Parameter
        {
            Id = ClassId,
            Name = "Parameter1",
            Type = "string"
        };
    }
}
