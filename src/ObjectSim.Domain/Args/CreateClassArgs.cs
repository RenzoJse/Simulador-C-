using ObjectSim.Domain;

namespace ObjectSim.BusinessLogic.Args;

public class CreateClassArgs(
    string? name,
    bool isAbstract,
    bool isSealed,
    List<Attribute> attribute,
    List<Method> methods,
    Class parent,
    Guid id)
{
    public string? Name { get; set; } = name;
    public bool IsAbstract { get; set; } = isAbstract;
    public bool IsSealed { get; set; } = isSealed;
    public List<Attribute> Attribute { get; set; } = attribute;
    public List<Method> Methods { get; set; } = methods;
    public Class Parent { get; set; } = parent;
    public Guid Id { get; set; } = id;
}
