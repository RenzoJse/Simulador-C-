using DataType = ObjectSim.Domain.DataType;

namespace ObjectSim.WebApi.DTOs.Out;

public record DataTypeInformationDtoOut
{
    public Guid Id { get; init; }
    public string Type { get; init; } = string.Empty;

    public static DataTypeInformationDtoOut ToInfo(DataType dataType)
    {
        return new DataTypeInformationDtoOut
        {
            Id = dataType.Id,
            Type = dataType.Type
        };
    }
}
