namespace ObjectSim.Domain;
public abstract class DataType
{
    public Guid Id { get; init; }
    public string Name { get; protected init; } = null!;
}
