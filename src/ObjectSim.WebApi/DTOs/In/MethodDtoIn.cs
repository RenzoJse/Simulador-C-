using ObjectSim.Domain;
using ObjectSim.Domain.Args;

namespace ObjectSim.WebApi.DTOs.In;

public class MethodDtoIn
{
    public required string Name { get; init; }
    public required string Type { get; init; }
    public required string Accessibility { get; init; }
    public bool IsAbstract { get; init; }
    public bool IsSealed { get; init; }
    public bool IsOverride { get; init; }
    public List<LocalVariable> LocalVariables { get; init; } = [];
    public List<Parameter> Parameters { get; init; } = [];
    public List<Method> Methods { get; init; } = [];

    public CreateMethodArgs ToArgs()
    {
        ArgumentNullException.ThrowIfNull(Name, nameof(Name));
        return new CreateMethodArgs(Name, Type, Accessibility, IsAbstract, IsSealed, IsOverride, LocalVariables, Parameters, Methods);
    }
}
