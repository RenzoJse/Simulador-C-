namespace ObjectSim.Domain.Args;

public class CreateDataTypeArgs(
    string name, string kind)
{
    public string Name { get; set; } = name;
    public string Kind { get; set; } = kind;
}
