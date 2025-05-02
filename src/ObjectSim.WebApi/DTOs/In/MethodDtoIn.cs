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
    public string? ClassId { get; set; }
    public List<LocalVariable> LocalVariables { get; init; } = [];
    public List<Parameter> Parameters { get; init; } = [];
    public List<Guid> InvokeMethodsId { get; init; } = [];

    public CreateMethodArgs ToArgs()
    {
        return new CreateMethodArgs(Name, Type, Accessibility,
            IsAbstract, IsSealed, IsOverride, Guid.Parse(ClassId!), LocalVariables, Parameters, InvokeMethodsId);
    }
}
