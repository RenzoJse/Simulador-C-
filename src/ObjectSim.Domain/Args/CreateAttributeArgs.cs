namespace ObjectSim.Domain.Args;

public class CreateAttributeArgs(
    IDataType dataType,
    Attribute.AttributeVisibility? visibility,
    Guid classId,
    string name)
{
    public IDataType DataType { get; set; } = dataType;
    public Attribute.AttributeVisibility? Visibility { get; set; } = visibility;
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ClassId { get; set; } = classId;
    public string Name { get; set; } = name;
}
