namespace ObjectSim.Domain;
public class Parameter
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string? Name { get; set; } = null;
    public string? Type { get; set; } = null;
}
