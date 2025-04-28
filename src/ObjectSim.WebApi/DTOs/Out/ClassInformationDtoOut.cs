using ObjectSim.Domain;

namespace ObjectSim.WebApi.DTOs.Out;

public class ClassInformationDtoOut
{
    public required string Name { get; init; }
    public bool IsAbstract { get; init; }
    public bool IsInterface { get; init; }
    public bool IsSealed { get; init; }
    public List<string> Attributes { get; init; } = [];
    public List<string> Methods { get; init; } = [];
    public Guid? Parent { get; init; }

    public static ClassInformationDtoOut ToInfo(Class classInfo)
    {
        return new ClassInformationDtoOut
        {
            Name = classInfo.Name!,
            IsAbstract = (bool)classInfo.IsAbstract!,
            IsInterface = (bool)classInfo.IsInterface!,
            IsSealed = (bool)classInfo.IsSealed!,
            Attributes = classInfo.Attributes!.Select(attribute => attribute.Name).ToList()!,
            Methods = classInfo.Methods!.Select(method => method.Name).ToList()!,
            Parent = classInfo.Parent?.Id
        };
    }
}
