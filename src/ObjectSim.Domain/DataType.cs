namespace ObjectSim.Domain;
public abstract class DataType
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; protected init; } = null!;
    public string Type = null!;
    public List<Method> Methods { get; init; } = [];
    public List<Guid> MethodIds { get; init; } = [];
}
