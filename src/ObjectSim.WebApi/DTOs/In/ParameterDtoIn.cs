using ObjectSim.Domain;

namespace ObjectSim.WebApi.DTOs.In;

public class ParameterDtoIn
{
    public required string Name { get; init; }
    public required string Type { get; init; }

    public Parameter ToEntity()
    {
        var result = new Parameter
        {
            Name = Name,
            Type = Enum.Parse<Parameter.ParameterDataType>(Type),
        };

        return result;
    }
}
