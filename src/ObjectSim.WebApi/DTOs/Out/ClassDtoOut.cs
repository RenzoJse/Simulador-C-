using ObjectSim.Domain;

namespace ObjectSim.WebApi.DTOs.Out;

public record ClassDtoOut(Class ClassInfo)
{
    public string Name { get; } = ClassInfo.Name!;
    public bool? IsAbstract { get; } = ClassInfo.IsAbstract;
    public bool? IsInterface { get; } = ClassInfo.IsInterface;
    public bool? IsSealed { get; } = ClassInfo.IsSealed;
    public List<string> Attributes { get; } = []; 
    public List<string> Methods { get; } = [];
    public Guid? Parent { get; }
    public Guid Id { get; }
}
