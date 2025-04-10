using ObjectSim.Domain;
using Attribute = ObjectSim.Domain.Attribute;

namespace ClassManagement;

public class Class
{
    public string? Name { get; set; }
    public bool IsAbstract { get; set; }
    public bool IsSealed { get; set; }
    public List<Attribute>? Attributes { get; set; }
    public List<Method>? Methods { get; set; }
    public string? Parent { get; set; }
    public Guid Id { get; set; } = Guid.NewGuid();
}
