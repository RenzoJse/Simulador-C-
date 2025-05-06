namespace ObjectSim.Domain;

public class InvokeMethod(Guid methodId, Guid invokeMethodId)
{
    public Guid MethodId { get; set; } = methodId;
    public Guid InvokeMethodId { get; set; } = invokeMethodId;
}
