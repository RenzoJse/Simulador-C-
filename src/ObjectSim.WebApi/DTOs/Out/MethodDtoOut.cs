using ObjectSim.Domain;

namespace ObjectSim.WebApi.DTOs.Out;

public class MethodDtoOut
{
    public required string Name { get; init; }
    public required string Type { get; init; }
    public required string Accessibility { get; init; }
    public bool IsAbstract { get; init; }
    public bool IsSealed { get; init; }
    public bool IsOverride { get; init; }
    public List<LocalVariable> LocalVariables { get; init; } = [];
    public List<Parameter> Parameters { get; init; } = [];
    public List<Method> Methods { get; init; } = [];

    public static MethodDtoOut ToInfo(Method methodInfo)
    {
        return new MethodDtoOut
        {
            Name = methodInfo.Name!,
            Type = methodInfo.Type.ToString(),
            Accessibility = methodInfo.Accessibility.ToString(),
            IsAbstract = methodInfo.Abstract,
            IsSealed = methodInfo.IsSealed,
            IsOverride = methodInfo.IsOverride,
            LocalVariables = methodInfo.LocalVariables ?? [],
            Parameters = methodInfo.Parameters ?? [],
            Methods = methodInfo.MethodsInvoke ?? []
        };
    }
}
