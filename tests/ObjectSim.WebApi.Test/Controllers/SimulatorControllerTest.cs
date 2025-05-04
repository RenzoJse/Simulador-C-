using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;
using ObjectSim.WebApi.Controllers;

namespace ObjectSim.WebApi.Test.Controllers;

[TestClass]
public  class SimulatorControllerTest
{
    private Mock<IMethodSimulatorService> _simulatorServiceMock = null!;
    private SimulatorController _simulatorController = null!;

    [TestInitialize]
    public void Setup()
    {
        _simulatorServiceMock = new Mock<IMethodSimulatorService>();
        _simulatorController = new SimulatorController(_simulatorServiceMock.Object);
    }

    [TestMethod]
    public void SimulateExecution_WhenValidArgs_ShouldReturnOkWithResult()
    {
        var args = new SimulateExecutionArgs
        {
            ReferenceClassId = Guid.NewGuid(),
            InstanceClassId = Guid.NewGuid(),
            MethodToExecuteId = Guid.NewGuid()
        };

        var expected = new List<string>
            {
                "Class1.this.Method1()",
                "Class2.this.Method2()"
            };

        _simulatorServiceMock
            .Setup(s => s.Simulate(args))
            .Returns(expected);

        var result = _simulatorController.SimulateExecution(args);

        var ok = result as OkObjectResult;
        ok.Should().NotBeNull();
        ok!.StatusCode.Should().Be(200);
        ok.Value.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void SimulateExecution_WhenExceptionThrown_ShouldThrow()
    {
        var args = new SimulateExecutionArgs();

        _simulatorServiceMock
            .Setup(s => s.Simulate(It.IsAny<SimulateExecutionArgs>()))
            .Throws(new Exception("Unexpected error"));

        Action act = () => _simulatorController.SimulateExecution(args);

        act.Should().Throw<Exception>().WithMessage("Unexpected error");
    }
}
