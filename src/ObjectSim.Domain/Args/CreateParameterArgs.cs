namespace ObjectSim.Domain.Args;
public class CreateParameterArgs(
    string name,
    string type)
{
    public string? Name { get; set; } = name;
    public string Type { get; set; } = type;
}
