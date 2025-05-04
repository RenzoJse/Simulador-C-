namespace ObjectSim.Domain;
public abstract class DataType
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public string Type { get; set; } = null!;
    public List<Guid> MethodIds { get; set; } = [];

    public virtual bool IsSameType(DataType other)
    {
        return Type == other.Type;
    }
}
