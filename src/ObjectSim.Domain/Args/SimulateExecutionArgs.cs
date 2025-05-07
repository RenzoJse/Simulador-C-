namespace ObjectSim.Domain.Args;
public class SimulateExecutionArgs
{
    public Guid ReferenceId { get; set; } = Guid.Empty;
    public Guid InstanceId { get; set; } = Guid.Empty;
    public Guid MethodId { get; set; } = Guid.Empty;
}
