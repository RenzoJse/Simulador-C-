namespace ObjectSim.Domain;
public class ValueType: IDataType
{
    public static readonly List<string> BuiltinTypes = ["int", "bool", "char", "decimal", "DateTime"];
    public string Name { get; private set; }
    private ValueType(string name) => Name = name;

    public static ValueType Create(string name)
    {
        return !BuiltinTypes.Contains(name) ? throw new ArgumentException($"Invalid ValueType: {name}") : new ValueType(name);
    }
}
