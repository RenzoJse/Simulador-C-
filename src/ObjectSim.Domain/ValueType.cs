namespace ObjectSim.Domain;
public class ValueType : DataType
{
    public static readonly List<string> BuiltinTypes = ["int", "bool", "char", "decimal", "DateTime"];
    private const int MaxNameLength = 20;

    public ValueType(string name, string type, List<Method> methods)
    {
        ValidateValueType(name, type);
        Name = name;
        Type = type;
    }

    private static void ValidateValueType(string name, string type)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name), "Name cannot be null or empty.");
        }
        if (name.Length > MaxNameLength)
        {
            throw new ArgumentException("Name cannot be longer than 20 characters.");
        }
        if (!name.All(char.IsLetter))
        {
            throw new ArgumentException("Name cannot contain special characters.");
        }
        if (!BuiltinTypes.Contains(type))
        {
            throw new ArgumentException($"Invalid ValueType: {type}.");
        }
    }
    
    public static ValueType Create(string name, string type)
    {
        return null!;
    }
}
