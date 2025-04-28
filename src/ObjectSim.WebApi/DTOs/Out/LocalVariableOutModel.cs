namespace ObjectSim.WebApi.DTOs.Out;

public class LocalVariableOutModel
{
    public required string Name { get; init; }
    public required string Type { get; init; }

    public static LocalVariableOutModel ToInfo(ObjectSim.Domain.Parameter parameter)
    {
        return new LocalVariableOutModel
        {
            Name = parameter.Name!,
            Type = parameter.Type.ToString()
        };
    }
}
