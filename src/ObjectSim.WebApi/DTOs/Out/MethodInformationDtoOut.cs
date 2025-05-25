using ObjectSim.Domain;

namespace ObjectSim.WebApi.DTOs.Out;

public record MethodInformationDtoOut(Method MethodInfo)
{
    public string Name { get; } = MethodInfo.Name!;
    public string Type { get; } = MethodInfo.TypeId.ToString();
    public string Accessibility { get; } = MethodInfo.Accessibility.ToString();
    public bool IsAbstract { get; } = MethodInfo.Abstract;
    public bool IsSealed { get; } = MethodInfo.IsSealed;
    public bool IsOverride { get; } = MethodInfo.IsOverride;
    public List<DataTypeInformationDtoOut> LocalVariables { get; } = MethodInfo.LocalVariables != null
            ? MethodInfo.LocalVariables.Select(DataTypeInformationDtoOut.ToInfo).ToList()
            : [];
    public List<DataTypeInformationDtoOut> Parameters { get; } = MethodInfo.Parameters != null
            ? MethodInfo.Parameters.Select(DataTypeInformationDtoOut.ToInfo).ToList()
            : [];
    public List<string> InvokeMethodsIds { get; } = MethodInfo.MethodsInvoke != null
        ? MethodInfo.MethodsInvoke.Select(m => m.InvokeMethodId.ToString()).ToList()
        : [];
}
