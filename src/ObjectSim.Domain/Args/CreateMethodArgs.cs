namespace ObjectSim.Domain.Args;
public class CreateMethodArgs(
    string name,
    bool? isAbstract,
    bool? isSealed,
    string accessibility,
    string type,
    List<Guid> localVariables,
    List<Guid> parameters)
{
    public string? Name { get; set; } = name;
    public bool? IsAbstract { get; set; } = isAbstract;
    public bool? IsSealed { get; set; } = isSealed;
    public string Accessibility { get; set; } = accessibility;
    public string Type { get; set; } = type;
    public List<Guid> Parameters { get; set; } = parameters;
    public List<Guid> LocalVariables { get; set; } = localVariables;
}
