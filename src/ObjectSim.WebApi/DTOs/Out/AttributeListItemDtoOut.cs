namespace ObjectSim.WebApi.DTOs.Out;

public class AttributeListItemDtoOut
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;

    public static AttributeListItemDtoOut FromEntity(Domain.Attribute attribute)
    {
        return new AttributeListItemDtoOut
        {
            Id = attribute.Id,
            Name = attribute.Name!
        };
    }
}
