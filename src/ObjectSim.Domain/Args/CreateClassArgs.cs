namespace ObjectSim.Domain.Args;

public class CreateClassArgs
{
    public string? Name { get; set; }
    public bool? IsAbstract { get; set; }
    public bool? IsSealed { get; set; }
    public List<Guid> Attributes { get; set; }
    public List<Guid> Methods { get; set; }
    public Guid Parent { get; set; }

    public CreateClassArgs(string name, bool? isAbstract, bool? isSealed, List<Guid> attributes, List<Guid> methods, Guid parent)
    {
        Name = name;
        IsAbstract = isAbstract;
        IsSealed = isSealed;
        Attributes = attributes;
        Methods = methods;
        Parent = parent;
    }
}
