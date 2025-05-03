using DataType = ObjectSim.Domain.DataType;

namespace ObjectSim.WebApi.DTOs.Out;

public record DataTypeInformationDtoOut
{
    public string? Name { get; init;} = String.Empty;
    public string? Type { get; init;} = String.Empty;

    public static DataTypeInformationDtoOut ToInfo(DataType dataType)
    {
        return new DataTypeInformationDtoOut { Name = dataType.Name, Type = dataType.Type };
    }
}
