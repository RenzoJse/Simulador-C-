namespace ObjectSim.Domain.Args;

public class CreateAttributeArgs(
    Guid dataTypeId,
    string visibility,
    Guid classId,
    string name,
    bool isStatic)
{
    public Guid DataTypeId { get; set; } = dataTypeId;
    public string Visibility { get; set; } = visibility;
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ClassId { get; set; } = classId;
    public string Name { get; set; } = name;
    public bool IsStatic { get; set; } = isStatic;
}
