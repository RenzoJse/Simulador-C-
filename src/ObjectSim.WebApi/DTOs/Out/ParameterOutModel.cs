namespace ObjectSim.WebApi.DTOs.Out;

public class ParameterOutModel(ObjectSim.Domain.Parameter parameter)
{
    public string Name { get; init; } = parameter.Name!;
    public string Type { get; init; } = parameter.Type.ToString();
}
