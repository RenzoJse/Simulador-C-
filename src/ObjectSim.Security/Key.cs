namespace ObjectSim.Security;

public class Key
{
    public Key()
    {
        AccessKey = Guid.NewGuid();
    }

    public Guid AccessKey { get; }
}
