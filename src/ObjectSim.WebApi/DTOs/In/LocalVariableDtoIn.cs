using ObjectSim.Domain;

namespace ObjectSim.WebApi.DTOs.In;

public class LocalVariableDtoIn
{
    public required string Name { get; init; }
    public required string Type { get; init; }


    public LocalVariable ToEntity()
    {
        var result = new LocalVariable
        {
            Name = Name,
            Type = Enum.Parse<LocalVariable.LocalVariableDataType>(Type),
        };

        return result;
    }
}
