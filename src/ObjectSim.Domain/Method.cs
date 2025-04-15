namespace ObjectSim.Domain;

public class Method
{
    public Guid Id { get; set; }
    public string? Name { get; set; } = null;
    public string? Type { get; set; } = null;
    public bool Abstract { get; set; } = false;
    public bool IsSealed { get; set; } = false;
    public string? Accessibility { get; set; } = null;
    public List<Parameter> Parameters { get; set; } = [];
    public List<LocalVariable> LocalVariables { get; set; } = [];
}
