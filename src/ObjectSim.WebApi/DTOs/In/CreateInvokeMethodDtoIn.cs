using ObjectSim.Domain.Args;

namespace ObjectSim.WebApi.DTOs.In;

public record CreateInvokeMethodDtoIn
{
    public required string InvokeMethodId { get; init; }
    public required string Reference { get; init; }

    public CreateInvokeMethodArgs ToArgs()
    {
        ArgumentNullException.ThrowIfNull(InvokeMethodId, nameof(InvokeMethodId));
        ArgumentNullException.ThrowIfNull(Reference, nameof(Reference));
        return new CreateInvokeMethodArgs(Guid.Parse(InvokeMethodId), Reference);
    }
}
