namespace ObjectSim.WebApi.DTOs.In;

public class SimulateMethodExecutionDtoIn
{
    public Guid ReferenceClassId { get; init; }
    public Guid InstanceClassId { get; init; }
    public Guid MethodId { get; init; }
}
