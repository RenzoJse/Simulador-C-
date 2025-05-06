namespace ObjectSim.Domain;

public class InvokeMethod(Guid methodId, Guid invokeMethodId, string reference)
{
    public Guid MethodId { get; set; } = methodId;
    public Guid InvokeMethodId { get; set; } = invokeMethodId;
    public string Reference { get; set; } = reference;
}
