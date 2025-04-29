using ObjectSim.Domain;

namespace ObjectSim.WebApi.DTOs.Out;

public class ClassInformationDtoOut
{
    public required string Name { get; init; }
    public bool? IsAbstract { get; init; }
    public bool? IsInterface { get; init; }
    public bool? IsSealed { get; init; }
    public List<string> Attributes { get; init; } = [];
    public List<string> Methods { get; init; } = [];
    public Guid? Parent { get; init; }
    public Guid Id { get; init; }

    public static ClassInformationDtoOut ToInfo(Class classInfo)
    {
        return new ClassInformationDtoOut
        {
            Name = classInfo.Name!,
            IsAbstract = classInfo.IsAbstract!,
            IsInterface = classInfo.IsInterface!,
            IsSealed = classInfo.IsSealed!,
            Attributes = (classInfo.Attributes!.Select(attribute => attribute.Name).ToList() ?? [])!,
            Methods = (classInfo.Methods!.Select(method => method.Name).ToList() ?? [])!,
            Parent = classInfo.Parent?.Id,
            Id = classInfo.Id
        };
    }
}
