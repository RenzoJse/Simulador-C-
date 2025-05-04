namespace ObjectSim.WebApi.DTOs.Out;

public class ExecutionStepDtoOut
{
    public string Caller { get; init; } = string.Empty;
    public string MethodName {  get; init; } = string.Empty;
    public List<ExecutionStepDtoOut> NestedCalls { get; init; } = [];
}
