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
        Validate(classId, type);
        Id = classId;
        Type = type;
    }

    private static void Validate(Guid classId, string type)
    {
        if(string.IsNullOrWhiteSpace(classId.ToString()) || type is null)
        {
            throw new ArgumentException("ClassId or type cannot be empty or null.");
        }
    }
}
