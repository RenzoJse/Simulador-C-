using ObjectSim.Domain;
using ObjectSim.Domain.Args;

namespace ObjectSim.WebApi.DTOs.In;

public class MethodDtoIn
{
    public required string Name { get; init; }
    public required CreateDataTypeDtoIn Type { get; init; }
    public required string Accessibility { get; init; }
    public bool IsAbstract { get; init; }
    public bool IsSealed { get; init; }
    public bool IsOverride { get; init; }
    public string? ClassId { get; set; }
    public List<CreateDataTypeDtoIn> LocalVariables { get; init; } = [];
    public List<CreateDataTypeDtoIn> Parameters { get; init; } = [];
    public List<Guid> InvokeMethodsId { get; init; } = [];

    public CreateMethodArgs ToArgs()
    {
        List<CreateDataTypeArgs> localVariables = [];
        localVariables.AddRange(LocalVariables.Select(localVariable => new CreateDataTypeArgs(localVariable.Name, localVariable.Type)));
        List<CreateDataTypeArgs> parameters = [];
        parameters.AddRange(Parameters.Select(parameter => new CreateDataTypeArgs(parameter.Name, parameter.Type)));
        var type = new CreateDataTypeArgs(Type.Name, Type.Type);
        return new CreateMethodArgs(Name, type, Accessibility,
            IsAbstract, IsSealed, IsOverride, Guid.Parse(ClassId!), localVariables , parameters, InvokeMethodsId);
    }
}
