using ObjectSim.Domain;

namespace ObjectSim.BusinessLogic.Args;

public class CreateClassArgs
{
    public string? Name { get; set; }
    public bool IsAbstract { get; set; }
    public bool IsSealed { get; set; }
    public List<Attribute> Attribute { get; set; } = [];
    public List<Method> Methods { get; set; } = [];
    public Class Parent { get; set; } = new();
    public Guid Id { get; set; } = Guid.NewGuid();

    public CreateClassArgs(string? name, bool isAbstract, bool isSealed, List<Attribute> attribute,
        List<Method> methods, Class parent, Guid id)
    {
        Name = name;
        IsAbstract = isAbstract;
        IsSealed = isSealed;
        Attribute = attribute;
        Methods = methods;
        Parent = parent;
        Id = id;
    }
}
