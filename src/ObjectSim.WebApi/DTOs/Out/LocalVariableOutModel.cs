public class LocalVariableOutModel(ObjectSim.Domain.LocalVariable localVariable)
{
    public string Name { get; init; } = localVariable.Name!;
    public string Type { get; init; } = localVariable.Type.ToString();
}
