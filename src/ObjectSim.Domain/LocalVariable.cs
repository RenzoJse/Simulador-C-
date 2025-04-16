namespace ObjectSim.Domain;
public class LocalVariable
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Name { get; set; } = null;
    public string? Type { get; set; } = null;
}
