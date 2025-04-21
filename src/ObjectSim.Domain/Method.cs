namespace ObjectSim.Domain;

public class Method
{
    public enum MethodDataType
    {
        String,
        Char,
        Int,
        Decimal,
        Bool,
        DateTime
    }
    public enum MethodAccessibility
    {
        Public,
        Private,
        Protected,
        Internal,
        ProtectedInternal,
        PrivateProtected
    }
    public Guid Id { get; set; }
    public string? Name { get; set; } = null;
    public MethodDataType? Type { get; set; } = null;
    public bool Abstract { get; set; } = false;
    public bool IsSealed { get; set; } = false;
    public MethodAccessibility? Accessibility { get; set; } = null;
    public List<Parameter> Parameters { get; set; } = [];
    public List<LocalVariable> LocalVariables { get; set; } = [];
}
