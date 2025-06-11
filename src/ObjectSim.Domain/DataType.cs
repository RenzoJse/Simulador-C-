namespace ObjectSim.Domain;
public abstract class DataType
{
    public Guid Id { get; set; }
    public string Type { get; set; } = null!;
    public virtual bool IsSameType(DataType other)
    {
        return Type == other.Type;
    }
}
