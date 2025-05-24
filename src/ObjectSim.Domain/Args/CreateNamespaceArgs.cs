

namespace ObjectSim.Domain.Args;
public class CreateNamespaceArgs(string name, Guid? parentId)
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = name;
    public Guid? ParentId { get; set; } = parentId;
}
