namespace ObjectSim.WebApi.DTOs.Out;

public class ParameterOutModel
{
    public required string Name { get; init; }
    public required string Type { get; init; }

    public static ParameterOutModel ToInfo(ObjectSim.Domain.Parameter parameter)
    {
        return new ParameterOutModel
        {
            Name = parameter.Name!,
            Type = parameter.Type.ToString()
        };
    }
}
