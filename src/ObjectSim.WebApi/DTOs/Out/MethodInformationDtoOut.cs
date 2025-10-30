using ObjectSim.Domain;

namespace ObjectSim.WebApi.DTOs.Out;

public record MethodInformationDtoOut(Method MethodInfo)
{
    public string Id { get; } = MethodInfo.Id.ToString();
    public string Name { get; } = MethodInfo.Name!;
    public string Type { get; } = MethodInfo.TypeId.ToString();
    public string Accessibility { get; } = MethodInfo.Accessibility.ToString();
    public bool IsAbstract { get; } = MethodInfo.Abstract;
    public bool IsSealed { get; } = MethodInfo.IsSealed;
    public bool IsOverride { get; } = MethodInfo.IsOverride;
    public List<VariableInformatioDtoOut> LocalVariables { get; } = MethodInfo.LocalVariables != null
            ? MethodInfo.LocalVariables.Select(VariableInformatioDtoOut.ToInfo).ToList()
            : [];
    public List<VariableInformatioDtoOut> Parameters { get; } = MethodInfo.Parameters != null
            ? MethodInfo.Parameters.Select(VariableInformatioDtoOut.ToInfo).ToList()
            : [];
    public List<string> InvokeMethodsIds { get; } = MethodInfo.MethodsInvoke != null
        ? MethodInfo.MethodsInvoke.Select(m => m.InvokeMethodId.ToString()).ToList()
        : [];
}
