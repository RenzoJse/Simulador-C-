using ObjectSim.Domain.Args;

namespace ObjectSim.WebApi.DTOs.In;

public class CreateMethodDtoIn
{
    public required string Name { get; init; }
    public required string? Type { get; init; }
    public required string Accessibility { get; init; }
    public bool IsAbstract { get; init; }
    public bool IsSealed { get; init; }
    public bool IsOverride { get; init; }
    public bool? IsVirtual { get; init; }
    public bool? IsStatic { get; init; }
    public string? ClassId { get; init; }
    public List<CreateVariableDtoIn> LocalVariables { get; init; } = [];
    public List<CreateVariableDtoIn> Parameters { get; init; } = [];
    public List<Guid> InvokeMethodsId { get; init; } = [];

    public CreateMethodArgs ToArgs()
    {
        List<CreateVariableArgs> localVariables = [];
        localVariables.AddRange(LocalVariables.Select(localVariable => new CreateVariableArgs(Guid.Parse(ClassId!), localVariable.Name)));
        List<CreateVariableArgs> parameters = [];
        parameters.AddRange(Parameters.Select(parameter => new CreateVariableArgs(Guid.Parse(ClassId!), parameter.Name)));
        return new CreateMethodArgs(Name, Guid.Parse(Type!), Accessibility,
            IsAbstract, IsSealed, IsOverride, IsVirtual, IsStatic, Guid.Parse(ClassId!), localVariables, parameters, InvokeMethodsId);
    }
}
