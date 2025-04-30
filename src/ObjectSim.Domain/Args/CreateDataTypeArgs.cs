namespace ObjectSim.Domain.Args;

public class CreateDataTypeArgs(
    string name)
{
    public string? Name { get; set; } = name;
}
