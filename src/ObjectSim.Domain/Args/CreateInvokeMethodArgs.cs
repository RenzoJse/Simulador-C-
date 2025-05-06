namespace ObjectSim.Domain.Args;

public record CreateInvokeMethodArgs(Guid invokeMethodId, Guid methodId, string reference)
{
    public Guid InvokeMethodId { get; init; } =  invokeMethodId;
    public Guid MethodId { get; init; } = methodId;
    public string Reference { get; init; } = reference;
}
