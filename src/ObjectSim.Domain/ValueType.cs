namespace ObjectSim.Domain;
public class ValueType : DataType
{
    public static readonly List<string> BuiltinTypes = ["int", "bool", "char", "decimal", "DateTime"];

    public ValueType(string name, string type, List<Method> methods)
    {
        Name = name;
    }

    public static ValueType Create(string name, string type)
    {
        if (!BuiltinTypes.Contains(type))
        {
            throw new ArgumentException($"Invalid ValueType: {type}.");
        }

        return null!;
    }
}
