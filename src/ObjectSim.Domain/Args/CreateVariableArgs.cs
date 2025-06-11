namespace ObjectSim.Domain.Args;

public class CreateVariableArgs(Guid classId, string name)
{
    public Guid ClassId { get; set; } = classId;
    public string Name { get; set; } = name;
}
