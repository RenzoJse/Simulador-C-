namespace ObjectSim.Domain.Args;

public class CreateClassArgs(
    string name,
    bool? isAbstract,
    bool? isSealed,
    List<Guid> attributes,
    List<Guid> methods,
    Guid parent)
{
    public string? Name { get; set; } = name;
    public bool? IsAbstract { get; set; } = isAbstract;
    public bool? IsSealed { get; set; } = isSealed;
    public List<Guid> Attributes { get; set; } = attributes;
    public List<Guid> Methods { get; set; } = methods;
    public Guid Parent { get; set; } = parent;
}
