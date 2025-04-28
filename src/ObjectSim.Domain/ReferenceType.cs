namespace ObjectSim.Domain;
public class ReferenceType : DataType
{
    public static readonly List<string> BuiltinTypes = ["string", "object"];
    private ReferenceType(string name)
    {
        Name = name;
    }
    public static ReferenceType Create(string name)
    {
        return !BuiltinTypes.Contains(name) ? throw new ArgumentException($"Invalid ReferenceType: {name}") : new ReferenceType(name);
    }
}
