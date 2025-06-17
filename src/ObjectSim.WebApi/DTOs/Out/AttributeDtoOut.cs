namespace ObjectSim.WebApi.DTOs.Out;

public class AttributeDtoOut
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public Guid DataTypeId { get; set; }
    public string Visibility { get; set; } = null!;
    public Guid ClassId { get; set; }
    public bool IsStatic { get; set; }
    public static AttributeDtoOut ToInfo(Domain.Attribute attribute)
    {
        return new AttributeDtoOut
        {
            Id = attribute.Id,
            Name = attribute.Name!,
            Visibility = attribute.Visibility.ToString(),
            DataTypeId = attribute.DataTypeId,
            ClassId = attribute.ClassId,
            IsStatic = attribute.IsStatic
        };
    }
}
