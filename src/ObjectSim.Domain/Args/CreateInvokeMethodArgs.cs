namespace ObjectSim.Domain.Args;

public class CreateInvokeMethodArgs(Guid invokeMethodId, Guid MethodId)
{
    public Guid InvokeMethodId { get; set; } =  invokeMethodId;
    public Guid MethodId { get; set; } = MethodId;
}
