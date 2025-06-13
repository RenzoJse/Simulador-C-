namespace ObjectSim.Domain.Args;
public class CreateMethodArgs(
    string name,
    Guid typeId,
    string accessibility,
    bool? isAbstract,
    bool? isSealed,
    bool? isOverride,
    bool? isVirtual,
    bool? isStatic,
    Guid classId,
    List<CreateVariableArgs> localVariables,
    List<CreateVariableArgs> parameters,
    List<Guid> invokeMethods)
{
    public string? Name { get; set; } = name;
    public Guid TypeId { get; set; } = typeId;
    public string Accessibility { get; set; } = accessibility;
    public bool? IsAbstract { get; set; } = isAbstract;
    public bool? IsSealed { get; set; } = isSealed;
    public bool? IsOverride { get; set; } = isOverride;
    public bool? IsVirtual { get; set; } = isVirtual;
    public bool? IsStatic { get; set; } = isStatic;
    public Guid ClassId { get; set; } = classId;
    public List<CreateVariableArgs> LocalVariables { get; set; } = localVariables;
    public List<CreateVariableArgs> Parameters { get; set; } = parameters;
    public List<Guid> InvokeMethods { get; set; } = invokeMethods;
}
