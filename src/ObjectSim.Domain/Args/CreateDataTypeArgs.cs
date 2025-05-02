namespace ObjectSim.Domain.Args;

public class CreateDataTypeArgs(
    string name, string kind)
{
    public string Name { get; } = name;
    public string Kind { get; } = kind;
}
