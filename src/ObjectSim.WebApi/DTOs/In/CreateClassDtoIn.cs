using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using Attribute = ObjectSim.Domain.Attribute;

namespace ObjectSim.WebApi.DTOs.In;

public record CreateClassDtoIn
{
    public required string Name { get; init; }
    public bool IsAbstract { get; init; }
    public bool IsInterface { get; init; }
    public bool IsSealed { get; init; }
    public List<CreateAttributeArgs> Attributes { get; init; } = [];
    public List<CreateMethodArgs> Methods { get; init; } = [];
    public Guid? Parent { get; init; }

    public CreateClassArgs ToArgs()
    {
        ArgumentNullException.ThrowIfNull(Name, nameof(Name));
        return new CreateClassArgs(Name, IsAbstract, IsSealed, IsInterface, Attributes, Methods, Parent);
    }
}
