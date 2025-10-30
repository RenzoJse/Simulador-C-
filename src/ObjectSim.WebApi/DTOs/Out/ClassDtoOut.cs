using ObjectSim.Domain;

namespace ObjectSim.WebApi.DTOs.Out;

public record ClassDtoOut
{
    public string Name { get; init; } = string.Empty;
    public bool? IsAbstract { get; init; }
    public bool? IsInterface { get; init; }
    public bool? IsSealed { get; init; }
    public List<string> Attributes { get;  init; } = [];
    public List<string> Methods { get;  init; } = [];
    public Guid? Parent { get; init; }
    public Guid Id { get;  init; }

    public static ClassDtoOut ToInfo(Class classInfo)
    {
        return new ClassDtoOut
        {
            Id = classInfo.Id,
            Name = classInfo.Name ?? string.Empty,
            IsAbstract = classInfo.IsAbstract,
            IsInterface = classInfo.IsInterface,
            IsSealed = classInfo.IsSealed,
            Attributes = classInfo.Attributes!.Select(a => a.Name).ToList()!,
            Methods = classInfo.Methods!.Select(m => m.Name).ToList()!,
            Parent = classInfo.Parent?.Id
        };
    }
}
