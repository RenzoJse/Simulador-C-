using ObjectSim.Domain.Args;

namespace ObjectSim.WebApi.DTOs.In;

public record CreateClassDtoIn
{
    public required string Name { get; init; } = String.Empty;
    public bool IsAbstract { get; init; } = false;
    public bool IsInterface { get; init; } = false;
    public bool IsSealed { get; init; } = false;
    public List<CreateAttributeArgs> Attributes { get; init; } = [];
    public List<CreateMethodArgs> Methods { get; init; } = [];
    public string? Parent { get; init; } = String.Empty;

    public CreateClassArgs ToArgs()
    {
        ArgumentNullException.ThrowIfNull(Name, nameof(Name));
        Guid? parentId = string.IsNullOrWhiteSpace(Parent) ? null : Guid.Parse(Parent);
        return new CreateClassArgs(Name, IsAbstract, IsSealed, IsInterface, Attributes, Methods, parentId);
    }
}
