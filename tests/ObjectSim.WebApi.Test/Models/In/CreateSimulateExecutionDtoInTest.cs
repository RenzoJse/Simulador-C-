using FluentAssertions;
using ObjectSim.WebApi.DTOs.In;

namespace ObjectSim.WebApi.Test.Models.In;

[TestClass]
public class CreateSimulateExecutionDtoInTest
{
    [TestMethod]
    public void Properties_SetAndGet_ShouldReturnExpectedValues()
    {
        var referenceId = Guid.NewGuid().ToString();
        var instanceId = Guid.NewGuid().ToString();
        var methodId = Guid.NewGuid().ToString();

        var dto = new CreateSimulateExecutionDtoIn
        {
            ReferenceId = referenceId,
            InstanceId = instanceId,
            MethodId = methodId
        };

        dto.ReferenceId.Should().Be(referenceId);
        dto.InstanceId.Should().Be(instanceId);
        dto.MethodId.Should().Be(methodId);
    }

    [TestMethod]
    public void ToArgs_WithValidGuids_ShouldReturnSimulateExecutionArgs()
    {
        var referenceId = Guid.NewGuid();
        var instanceId = Guid.NewGuid();
        var methodId = Guid.NewGuid();

        var dto = new CreateSimulateExecutionDtoIn
        {
            ReferenceId = referenceId.ToString(),
            InstanceId = instanceId.ToString(),
            MethodId = methodId.ToString()
        };

        var args = dto.ToArgs();

        args.ReferenceId.Should().Be(referenceId);
        args.InstanceId.Should().Be(instanceId);
        args.MethodId.Should().Be(methodId);
    }
}
