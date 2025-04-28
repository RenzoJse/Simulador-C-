public class LocalVariableOutModel
{
    public string Name { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;

    public LocalVariableOutModel(ObjectSim.Domain.LocalVariable localVariable)
    {
        Name = localVariable.Name!;
        Type = localVariable.Type.ToString();
    }
}
