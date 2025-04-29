namespace ObjectSim.Domain.Args;

public class CreateDataTypeArgs(
    string name)
{
    string Name { get; set; } = name;
}
