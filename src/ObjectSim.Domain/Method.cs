namespace ObjectSim.BusinessLogic;

public class Method
{
    public string Name { get; set; } = null;
    public string Type { get; set; } = null;
    public bool Abstract { get; set; } = false;
    public bool IsSealed { get; set; } = false;
    public string Accessibility { get; set; } = null;
}
