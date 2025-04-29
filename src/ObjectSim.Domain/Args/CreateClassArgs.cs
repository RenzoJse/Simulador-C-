namespace ObjectSim.Domain.Args;

public class CreateClassArgs(
    string name,
    bool? isAbstract,
    bool? isSealed,
    bool? isInterface,
    List<CreateAttributeArgs> attributes,
    List<Method> methods,
    Guid? parent)
{
    public string? Name { get; set; } = name;
    public bool? IsAbstract { get; set; } = isAbstract;
    public bool? IsSealed { get; set; } = isSealed;
    public bool? IsInterface { get; set; } = isInterface;
    public List<CreateAttributeArgs> Attributes { get; set; } = attributes;
    public List<Method> Methods { get; set; } = methods;
    public Guid? Parent { get; set; } = parent;
}
