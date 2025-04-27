using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain;

namespace ObjectSim.BusinessLogic.Test;

[TestClass]
public class ParameterServiceTest
{
    private Mock<IRepository<Parameter>>? _parameterRepositoryMock;
    private ParameterService? _parameterService;
    private static readonly Guid ParameterId = Guid.NewGuid();
    private Parameter? _parameter;


    [TestInitialize]
    public void Setup()
    {
        _parameterRepositoryMock = new Mock<IRepository<Parameter>>(MockBehavior.Strict);
        _parameterService = new ParameterService(_parameterRepositoryMock.Object);
        _parameter = new Parameter
        {
            Id = ParameterId,
            Name = "Parameter1",
            Type = Parameter.ParameterDataType.String
        };
    }

    [TestMethod]
    public void CreateValidParameter()
    {

        _parameterRepositoryMock!.Setup(repo => repo.Exists(It.IsAny<Expression<Func<Parameter, bool>>>())).Returns(false);
        _parameterRepositoryMock.Setup(repo => repo.Add(It.IsAny<Parameter>())).Returns((Parameter act) => act);

        var result = _parameterService!.Create(_parameter!);

        result.Should().NotBeNull();
        _parameterRepositoryMock.VerifyAll();
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]

    public void CreateNullParameter()
    {
        _parameterService!.Create(null!);
    }
}
