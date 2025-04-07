using ObjectSim.BusinessLogic;
using Attribute = ObjectSim.BusinessLogic.Attribute;

namespace ObjectSim.Domain;

public class Class
{
    public string? Name { get; set; }
    public string? Namespace { get; set; }
    public List<Attribute> Attributes { get; set; } = null!;
    public List<Method> Methods { get; set; } = null!;
    public List<string> Usings { get; set; } = null!;
    public string? BaseClass { get; set; }
    public bool IsAbstract { get; set; }
    public bool IsStatic { get; set; }
    public bool IsPartial { get; set; }
}
