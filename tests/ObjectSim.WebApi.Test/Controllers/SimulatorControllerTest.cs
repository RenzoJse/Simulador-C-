using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;
using ObjectSim.WebApi.Controllers;
using ObjectSim.WebApi.DTOs.In;

namespace ObjectSim.WebApi.Test.Controllers;

[TestClass]
public class SimulatorControllerTest
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
        var args = new CreateSimulateExecutionDtoIn()
        {
            ReferenceId = Guid.NewGuid().ToString(),
            InstanceId = Guid.NewGuid().ToString(),
            MethodId = Guid.NewGuid().ToString()
        };

        const string expected = "Execution: \nInstance.MainMethod() -> Instance.MainMethod() -> \nthis.FirstInvoked() -> other.SecondInvoked() -> ";

        _simulatorServiceMock
            .Setup(s => s.Simulate(It.IsAny<SimulateExecutionArgs>()))
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
        var args = new CreateSimulateExecutionDtoIn
        {
            ReferenceId = Guid.NewGuid().ToString(),
            InstanceId = Guid.NewGuid().ToString(),
            MethodId = Guid.NewGuid().ToString()
        };

        _simulatorServiceMock
            .Setup(s => s.Simulate(It.IsAny<SimulateExecutionArgs>()))
            .Throws(new Exception("Unexpected error"));

        Action act = () => _simulatorController.SimulateExecution(args);
        act.Should().Throw<Exception>().WithMessage("Unexpected error");
    }

    [TestMethod]
    public void SimulateExecution_NullDto_ShouldThrowNullReferenceException()
    {
        Action act = () => _simulatorController.SimulateExecution(null!);
        act.Should().Throw<NullReferenceException>();
    }

    [TestMethod]
    public void SimulateExecution_InvalidGuidStrings_ShouldThrowFormatException()
    {
        var badDto = new CreateSimulateExecutionDtoIn
        {
            ReferenceId = "not-a-guid",
            InstanceId = "also-bad",
            MethodId = "123"
        };

        Action act = () => _simulatorController.SimulateExecution(badDto);

        act.Should().Throw<FormatException>();
    }
}
