namespace ObjectSim.Domain.Args;

public class CreateDataTypeArgs(
    Guid classId, string type)
{
    public Guid ClassId { get; set; } = classId;
    public string Type { get; set; } = type;
}
