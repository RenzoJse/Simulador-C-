namespace ObjectSim.Domain.Args;
public class SimulateExecutionArgs
{
    public string ReferenceType { get; set; } = null!;
    public string InstanceType { get; set; } = null!;
    public string MethodName { get; set; } = null!;
}
