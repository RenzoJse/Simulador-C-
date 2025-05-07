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
    public void SimulateExecution_ShouldReturnOkResult_WithTrace()
    {
        var args = new SimulateExecutionArgs
        {
            ReferenceType = "ReferenceClass",
            InstanceType = "Instance",
            MethodId = Guid.NewGuid()
        };

        const string expected = "Execution: \nInstance.MainMethod() -> Instance.MainMethod() -> \nthis.FirstInvoked() -> other.SecondInvoked() -> ";

        _simulatorServiceMock
            .Setup(s => s.Simulate(args))
            .Returns(expected);

        var result = _simulatorController.SimulateExecution(args);

        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);

        var trace = okResult.Value as string;
        trace.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void SimulateExecution_WhenExceptionThrown_ShouldThrowException()
    {
        var args = new SimulateExecutionArgs
        {
            ReferenceType = "ReferenceClass",
            InstanceType = "InstanceClass",
            MethodId = Guid.NewGuid()
        };

        _simulatorServiceMock
            .Setup(s => s.Simulate(It.IsAny<SimulateExecutionArgs>()))
            .Throws(new Exception("Unexpected error"));

        Action act = () => _simulatorController.SimulateExecution(args);
        act.Should().Throw<Exception>().WithMessage("Unexpected error");
    }
}
