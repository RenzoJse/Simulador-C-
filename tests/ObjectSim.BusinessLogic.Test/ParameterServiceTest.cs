using System.Linq.Expressions;
using FluentAssertions;
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
        _parameterService = new ParameterService(_parameterRepositoryMock.Object);
        parameter = new Parameter
        {
            Id = ClassId,
            Name = "Parameter1",
            Type = "string"
        };
    }

    [TestMethod]
    public void CreateValidParameter()
    {

        _parameterRepositoryMock!.Setup(repo => repo.Exist(It.IsAny<Expression<Func<Parameter, bool>>>())).Returns(false);
        _parameterRepositoryMock.Setup(repo => repo.Add(It.IsAny<Parameter>())).Returns((Parameter act) => act);

        var result = _parameterService!.Create(parameter!);

        result.Should().NotBeNull();
        _parameterRepositoryMock.VerifyAll();
    }

    [TestMethod]
    public void CreateNullParameter()
    {
        _parameterService!.Create(null!);
    }
}
