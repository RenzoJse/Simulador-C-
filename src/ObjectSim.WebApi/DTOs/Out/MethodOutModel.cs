using ObjectSim.Domain;

namespace ObjectSim.WebApi.DTOs.Out;

public class MethodOutModel(Method methodInfo)
{
    public string Name { get; init; } = methodInfo.Name!;
    public string Type { get; init; } = methodInfo.Type.ToString();
    public string Accessibility { get; init; } = methodInfo.Accessibility.ToString();
    public bool IsAbstract { get; init; } = methodInfo.Abstract;
    public bool IsSealed { get; init; } = methodInfo.IsSealed;
    public bool IsOverride { get; init; } = methodInfo.IsOverride;
    public List<LocalVariableOutModel> LocalVariables { get; init; } = methodInfo.LocalVariables != null
            ? methodInfo.LocalVariables.Select(lv => new LocalVariableOutModel(lv)).ToList()
            : [];
    public List<ParameterOutModel> Parameters { get; init; } = methodInfo.Parameters != null
            ? methodInfo.Parameters.Select(p => new ParameterOutModel(p)).ToList()
            : [];
    public List<MethodOutModel> Methods { get; init; } = methodInfo.MethodsInvoke != null
            ? methodInfo.MethodsInvoke.Select(m => new MethodOutModel(m)).ToList()
            : [];
}
