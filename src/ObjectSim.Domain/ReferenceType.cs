namespace ObjectSim.Domain;
public class ReferenceType : DataType
{
    private ReferenceType()
    {
        Id = Guid.NewGuid();
        Name = string.Empty;
        Type = string.Empty;
        MethodIds = [];
    }

    public ReferenceType(string? name, string type, List<Guid> methodsIds)
    {
        Validate(name, methodsIds);

        Id = Guid.NewGuid();
        Name = name ?? "";
        Type = type;
        MethodIds = methodsIds;
    }

    private static void Validate(string? name, List<Guid> methodIds)
    {
        if(string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be null or empty.");
        }
        if(methodIds == null)
        {
            throw new ArgumentNullException(nameof(methodIds), "Methods cannot be null.");
        }
    }
}
