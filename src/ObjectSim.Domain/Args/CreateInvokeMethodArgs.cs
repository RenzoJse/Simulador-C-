namespace ObjectSim.Domain.Args;

public class CreateInvokeMethodArgs(Guid invokeMethodId)
{
    public Guid InvokeMethodId { get; set; } =  invokeMethodId; 
}
