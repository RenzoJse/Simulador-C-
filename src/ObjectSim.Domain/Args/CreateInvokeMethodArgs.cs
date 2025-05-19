namespace ObjectSim.Domain.Args;

public record CreateInvokeMethodArgs(Guid InvokeMethodId, string Reference)
{
    public Guid InvokeMethodId { get; init; } = InvokeMethodId;
    public string Reference { get; init; } = Reference;
}
