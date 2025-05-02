namespace ObjectSim.WebApi.DTOs.Out;

public class AttributeDtoOut
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string DataTypeName { get; set; } = null!;
    public string DataTypeKind { get; set; } = null!;
    public string Visibility { get; set; } = null!;
    public Guid ClassId { get; set; }
    public static AttributeDtoOut ToInfo(Domain.Attribute attribute)
    {
        return new AttributeDtoOut
        {
            Id = attribute.Id,
            Name = attribute.Name!,
            Visibility = attribute.Visibility.ToString(),
            DataTypeName = attribute.DataType.Name,
            DataTypeKind = attribute.DataType.Type,
            ClassId = attribute.ClassId
        };
    }
}
