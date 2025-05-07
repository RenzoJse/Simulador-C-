using ObjectSim.Domain.Args;

namespace ObjectSim.WebApi.DTOs.In;

public class CreateSimulateExecutionDtoIn
{
    public string ReferenceId { get; set; } = string.Empty;
    public string InstanceId { get; set; } = string.Empty;
    public string MethodId { get; set; } = string.Empty;

    public SimulateExecutionArgs ToArgs()
    {
        ArgumentNullException.ThrowIfNull(ReferenceId, nameof(ReferenceId));
        ArgumentNullException.ThrowIfNull(InstanceId, nameof(InstanceId));
        ArgumentNullException.ThrowIfNull(MethodId, nameof(MethodId));
        return new SimulateExecutionArgs
        {
            ReferenceId = Guid.Parse(ReferenceId),
            InstanceId = Guid.Parse(InstanceId),
            MethodId = Guid.Parse(MethodId)
        };
    }
}
