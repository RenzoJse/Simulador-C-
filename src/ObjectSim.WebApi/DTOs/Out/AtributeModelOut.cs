namespace ObjectSim.WebApi.DTOs.Out;

public class AtributeModelOut
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string DataTypeName { get; set; } = null!;
    public string DataTypeKind { get; set; } = null!;
    public string Visibility { get; set; } = null!;
    public Guid ClassId { get; set; }
}
