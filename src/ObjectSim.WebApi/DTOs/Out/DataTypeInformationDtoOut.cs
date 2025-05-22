using DataType = ObjectSim.Domain.DataType;

namespace ObjectSim.WebApi.DTOs.Out;

public record DataTypeInformationDtoOut
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public List<Guid> MethodIds { get; init; } = [];

    public static DataTypeInformationDtoOut ToInfo(DataType dataType)
    {
        return new DataTypeInformationDtoOut
        {
            Id = dataType.Id,
            Name = dataType.Name,
            Type = dataType.Type,
            MethodIds = dataType.MethodIds ?? []
        };
    }
}
