

namespace ObjectSim.Domain.Args;
public class CreateNamespaceArgs(string name, Guid? parentId, List<Guid> classIds)
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = name;
    public Guid? ParentId { get; set; } = parentId;
    public List<Guid> ClassIds { get; set; } = classIds;
}
