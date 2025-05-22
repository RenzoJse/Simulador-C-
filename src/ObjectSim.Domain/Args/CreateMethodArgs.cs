namespace ObjectSim.Domain.Args;
public class CreateMethodArgs(
    string name,
    CreateDataTypeArgs type,
    string accessibility,
    bool? isAbstract,
    bool? isSealed,
    bool? isOverride,
    bool? IsVirtual,
    Guid classId,
    List<CreateDataTypeArgs> localVariables,
    List<CreateDataTypeArgs> parameters,
    List<Guid> invokeMethods)
{
    public string? Name { get; set; } = name;
    public CreateDataTypeArgs Type { get; set; } = type;
    public string Accessibility { get; set; } = accessibility;
    public bool? IsAbstract { get; set; } = isAbstract;
    public bool? IsSealed { get; set; } = isSealed;
    public bool? IsOverride { get; set; } = isOverride;
    public bool? IsVirtual { get; set; } = IsVirtual;
    public Guid ClassId { get; set; } = classId;
    public List<CreateDataTypeArgs> LocalVariables { get; set; } = localVariables;
    public List<CreateDataTypeArgs> Parameters { get; set; } = parameters;
    public List<Guid> InvokeMethods { get; set; } = invokeMethods;
}
