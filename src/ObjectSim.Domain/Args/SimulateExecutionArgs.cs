namespace ObjectSim.Domain.Args;
public class SimulateExecutionArgs
{
    public Guid ReferenceClassId { get; set; }
    public Guid InstanceClassId { get; set; }
    public Guid MethodToExecuteId {  get; set; }
}
