namespace ObjectSim.Domain.Args;
public class CreateMethodArgs(
    string name,
    string type,
    string accessibility,
    bool? isAbstract,
    bool? isSealed,
    bool? isOverride,
    Guid classId,
    List<LocalVariable> localVariables,
    List<Parameter> parameters,
    List<Guid> invokeMethods)
{
    public string? Name { get; set; } = name;

    public string Type { get; set; } = type;
    public string Accessibility { get; set; } = accessibility;
    public bool? IsAbstract { get; set; } = isAbstract;
    public bool? IsSealed { get; set; } = isSealed;
    public bool? IsOverride { get; set; } = isOverride;
    public Guid ClassId { get; set; } = classId;
    public List<LocalVariable> LocalVariables { get; set; } = localVariables;
    public List<Parameter> Parameters { get; set; } = parameters;
    public List<Guid> InvokeMethods { get; set; } = invokeMethods;
}
