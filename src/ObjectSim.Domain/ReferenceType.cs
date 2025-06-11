namespace ObjectSim.Domain;
public class ReferenceType : DataType
{
    private ReferenceType()
    {
        Id = Guid.NewGuid();
        Type = string.Empty;
    }

    public ReferenceType(Guid classId, string type)
    {
        Validate(classId);
        Id = classId;
        Type = type;
    }

    private static void Validate(Guid classId)
    {
        if(string.IsNullOrWhiteSpace(classId.ToString()))
        {
            throw new ArgumentException("Name cannot be null or empty.");
        }
    }
}
