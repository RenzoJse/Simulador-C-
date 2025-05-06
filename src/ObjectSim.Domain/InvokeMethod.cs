namespace ObjectSim.Domain;

public class InvokeMethod(Guid methodId)
{
    public Guid MethodId { get; set; } = methodId;
}
