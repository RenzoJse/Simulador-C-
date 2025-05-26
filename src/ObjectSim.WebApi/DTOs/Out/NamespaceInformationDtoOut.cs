using ObjectSim.Domain;

namespace ObjectSim.WebApi.DTOs.Out;

public class NamespaceInformationDtoOut
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public Guid? ParentId { get; init; }

    public static NamespaceInformationDtoOut FromEntity(Namespace ns)
    {
        return new NamespaceInformationDtoOut
        {
            Id = ns.Id,
            Name = ns.Name,
            ParentId = ns.ParentId
        };
    }
}
