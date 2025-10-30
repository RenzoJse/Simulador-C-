using ObjectSim.Domain.Args;

namespace ObjectSim.WebApi.DTOs.In;

public class CreateNamespaceDtoIn
{
    public string Name { get; init; } = string.Empty;
    public Guid? ParentId { get; init; }
    public List<Guid> ClassIds { get; init; } = [];

    public CreateNamespaceArgs ToArgs()
    {
        return new CreateNamespaceArgs(Name, ParentId, ClassIds);
    }
}
