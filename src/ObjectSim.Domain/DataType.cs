namespace ObjectSim.Domain;
public abstract class DataType
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; protected init; } = null!;
    public string Type = null!;
    public List<Guid> MethodIds { get; protected init; } = [];

    public virtual bool IsSameType(DataType other)
    {
        return Type == other.Type;
    }
}
