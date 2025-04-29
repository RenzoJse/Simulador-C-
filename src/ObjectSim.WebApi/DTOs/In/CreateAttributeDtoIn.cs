namespace ObjectSim.WebApi.DTOs.In;

public class CreateAttributeDtoIn
{
    public string Name { get; set; } = null!;
    public string DataTypeName { get; set; } = null!;
    public string DataTypeKind { get; set; } = null!;
    public string Visibility { get; set; } = null!;
    public Guid ClassId { get; set; }
}
