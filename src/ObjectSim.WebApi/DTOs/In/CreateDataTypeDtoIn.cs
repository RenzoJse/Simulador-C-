using ObjectSim.Domain.Args;

namespace ObjectSim.WebApi.DTOs.In;

public record CreateDataTypeDtoIn
{
    public string Name { get; init; } = String.Empty;
    public string Type { get; init; } = String.Empty;
    
    public CreateDataTypeArgs ToArgs()
    {
        return new CreateDataTypeArgs(Name, Type);
    }
}
