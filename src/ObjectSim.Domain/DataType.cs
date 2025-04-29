namespace ObjectSim.Domain;
public abstract class DataType
{
    public Guid Id { get; set; }
    public string Name { get; protected set; } = null!;
}
