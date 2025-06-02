namespace ObjectSim.Domain;

public class Key
{
    public Guid AccessKey { get; init; } = Guid.NewGuid();
}
