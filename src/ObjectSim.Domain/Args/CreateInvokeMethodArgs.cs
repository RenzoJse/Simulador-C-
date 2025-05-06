namespace ObjectSim.Domain.Args;

public record CreateInvokeMethodArgs(Guid invokeMethodId, Guid methodId)
{
    public Guid InvokeMethodId { get; init; } =  invokeMethodId;
    public Guid MethodId { get; init; } = methodId;
}
