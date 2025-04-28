using ObjectSim.Domain;

namespace ObjectSim.WebApi.DTOs.Out;

public class MethodOutModel
{
    public string Name { get; init; } = null!;
    public string Type { get; init; } = null!;
    public string Accessibility { get; init; } = null!;
    public bool IsAbstract { get; init; }
    public bool IsSealed { get; init; }
    public bool IsOverride { get; init; }
    public List<LocalVariableOutModel> LocalVariables { get; init; } = [];
    public List<ParameterOutModel> Parameters { get; init; } = [];
    public List<MethodOutModel> Methods { get; init; } = [];

    public MethodOutModel(Method methodInfo)
    {
        Name = methodInfo.Name!;
        Type = methodInfo.Type.ToString();
        Accessibility = methodInfo.Accessibility.ToString();
        IsAbstract = methodInfo.Abstract;
        IsSealed = methodInfo.IsSealed;
        IsOverride = methodInfo.IsOverride;

        LocalVariables = methodInfo.LocalVariables != null
            ? methodInfo.LocalVariables.Select(lv => new LocalVariableOutModel(lv)).ToList()
            : [];

        Parameters = methodInfo.Parameters != null
            ? methodInfo.Parameters.Select(p => new ParameterOutModel(p)).ToList()
            : [];

        Methods = methodInfo.MethodsInvoke != null
            ? methodInfo.MethodsInvoke.Select(m => new MethodOutModel(m)).ToList()
            : [];
    }
}
