namespace ObjectSim.Domain;

public class Variable
{
    const int MinNameLength = 3;
    const int MaxNameLength = 20;

    public string Name { get; set; } = null!;
    public Guid TypeId { get; set; }
    public Guid VariableId { get; set; }
    public Guid MethodId { get; init; }
    public Method Method { get; init; } = null!;

    protected Variable() { }

    public Variable(Guid dataTypeId, string name, Method method)
    {
        ValidateNameNotNullOrWhitespace(name);
        ValidateNameLength(name);
        ValidateDataTypeId(dataTypeId);

        TypeId = dataTypeId;
        Name = name;
        VariableId = Guid.NewGuid();
        MethodId = method.Id;
        Method = method;
    }

    private static void ValidateNameNotNullOrWhitespace(string name)
    {
        if(string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be null or whitespace.", nameof(name));
        }
    }

    private static void ValidateNameLength(string name)
    {
        if(name.Length < MinNameLength)
        {
            throw new ArgumentException("Name cannot be that short.", nameof(name));
        }

        if(name.Length > MaxNameLength)
        {
            throw new ArgumentException("Name cannot be longer than 20 characters.", nameof(name));
        }
    }

    private static void ValidateDataTypeId(Guid dataTypeId)
    {
        if(dataTypeId == Guid.Empty)
        {
            throw new ArgumentException("Data type ID cannot be empty.", nameof(dataTypeId));
        }
    }
}
