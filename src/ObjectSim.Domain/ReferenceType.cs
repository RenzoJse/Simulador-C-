namespace ObjectSim.Domain;
public class ReferenceType : DataType
{
    private ReferenceType()
    {
        Id = Guid.NewGuid();
        Name = string.Empty;
        Type = string.Empty;
    }

    public ReferenceType(string? name, string type)
    {
        Validate(name);

        Id = Guid.NewGuid();
        Name = name ?? "";
        Type = type;
    }

    private static void Validate(string? name)
    {
        if(string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be null or empty.");
        }
    }
}
