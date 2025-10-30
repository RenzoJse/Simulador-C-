using ObjectSim.Domain.Args;

namespace ObjectSim.WebApi.DTOs.In;

public record CreateDataTypeDtoIn
{
    public string classId { get; init; } = String.Empty;
    public string Type { get; init; } = String.Empty;

    public CreateDataTypeArgs ToArgs()
    {
        return new CreateDataTypeArgs(Guid.Parse(classId), Type);
    }
}
