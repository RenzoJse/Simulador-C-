namespace ObjectSim.Domain;
public class ValueType : DataType
{
    public static readonly List<string> BuiltinTypes = ["int", "bool", "char", "decimal", "byte", "float", "double"];
    private const int MaxNameLength = 20;

    public ValueType()
    {
        Id = Guid.Empty;
        Type = string.Empty;
    }

    public ValueType(Guid classId, string type)
    {
        ValidateValueType(type);
        Id = classId;
        Type = type;
    }

    private static void ValidateValueType(string type)
    {
        ValidateNameNotNullOrEmpty(type);
        ValidateNameLength(type);
        ValidateNameCharacters(type);
        ValidateTypeIsBuiltin(type);
    }

    private static void ValidateNameNotNullOrEmpty(string name)
    {
        if(string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name), "Name cannot be null or empty.");
        }
    }

    private static void ValidateNameLength(string name)
    {
        if(name.Length > MaxNameLength)
        {
            throw new ArgumentException("Name cannot be longer than 20 characters.");
        }
    }

    private static void ValidateNameCharacters(string name)
    {
        if(!name.All(char.IsLetter))
        {
            throw new ArgumentException("Name cannot contain special characters.");
        }
    }

    private static void ValidateTypeIsBuiltin(string type)
    {
        if(!BuiltinTypes.Contains(type))
        {
            throw new ArgumentException($"Invalid ValueType: {type}.");
        }
    }
}
