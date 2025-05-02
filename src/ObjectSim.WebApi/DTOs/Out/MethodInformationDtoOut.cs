using ObjectSim.Domain;

namespace ObjectSim.WebApi.DTOs.Out;

public record MethodInformationDtoOut(Method methodInfo)
{
    public string Name { get; } = methodInfo.Name!;
    public string Type { get; } = methodInfo.Type.ToString();
    public string Accessibility { get; } = methodInfo.Accessibility.ToString();
    public bool IsAbstract { get; } = methodInfo.Abstract;
    public bool IsSealed { get; } = methodInfo.IsSealed;
    public bool IsOverride { get; } = methodInfo.IsOverride;
    public List<DataTypeInformationDtoOut> LocalVariables { get; } = methodInfo.LocalVariables != null
            ? methodInfo.LocalVariables.Select(DataTypeInformationDtoOut.ToInfo).ToList()
            : [];
    public List<DataTypeInformationDtoOut> Parameters { get; } = methodInfo.Parameters != null
            ? methodInfo.Parameters.Select(DataTypeInformationDtoOut.ToInfo).ToList()
            : [];
    public List<MethodInformationDtoOut> Methods { get; } = methodInfo.MethodsInvoke != null
            ? methodInfo.MethodsInvoke.Select(m => new MethodInformationDtoOut(m)).ToList()
            : [];
}
