namespace ObjectSim.Domain.Args;
public class CreateMethodArgs(
    string name,
    string type,
    string accessibility,
    bool? isAbstract,
    bool? isSealed,
    bool? isOverride,
    List<LocalVariable> localVariables,
    List<Parameter> parameters,
    List<Method> methods)
{
    public string? Name { get; set; } = name;

    public string Type { get; set; } = type;
    public string Accessibility { get; set; } = accessibility;
    public bool? IsAbstract { get; set; } = isAbstract;
    public bool? IsSealed { get; set; } = isSealed;
    public bool? IsOverride { get; set; } = isOverride;
    public List<LocalVariable> LocalVariables { get; set; } = localVariables;
    public List<Parameter> Parameters { get; set; } = parameters;
    public List<Method> Methods { get; set; } = methods;
}
