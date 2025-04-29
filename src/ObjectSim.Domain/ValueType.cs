namespace ObjectSim.Domain;
public class ValueType : DataType
{
    public static readonly List<string> BuiltinTypes = ["int", "bool", "char", "decimal", "DateTime"];
    private ValueType(string name)
    {
        Name = name;
    }
    public static ValueType Create(string name)
    {
        return !BuiltinTypes.Contains(name) ? throw new ArgumentException($"Invalid ValueType: {name}") : new ValueType(name);
    }
}
