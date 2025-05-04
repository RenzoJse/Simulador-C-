namespace ObjectSim.Domain;
public class ValueType : DataType
{
    public static readonly List<string> BuiltinTypes = ["int", "bool", "char", "decimal", "DateTime"];
    private const int MaxNameLength = 20;

    public ValueType()
    {
        Id = Guid.NewGuid();
        Name = string.Empty;
        Type = string.Empty;
        MethodIds = [];
    }

    public ValueType(string name, string type, List<Guid> methods)
    {
        ValidateValueType(name, type, methods);
        Name = name;
        Type = type;
        MethodIds = methods;
    }

    private static void ValidateValueType(string name, string type, List<Guid> methods)
    {
        ValidateNameNotNullOrEmpty(name);
        ValidateNameLength(name);
        ValidateNameCharacters(name);
        ValidateMethodsNotNull(methods);
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

    private static void ValidateMethodsNotNull(List<Guid> methods)
    {
        if(methods == null)
        {
            throw new ArgumentNullException(nameof(methods), "Methods cannot be null.");
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
