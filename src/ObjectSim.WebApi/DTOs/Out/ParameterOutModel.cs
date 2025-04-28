namespace ObjectSim.WebApi.DTOs.Out;

public class ParameterOutModel
{
    public string Name { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;

    public ParameterOutModel(ObjectSim.Domain.Parameter parameter)
    {
        Name = parameter.Name!;
        Type = parameter.Type.ToString();
    }
}
