namespace ObjectSim.Domain;

public class Variable
{
    public string Name { get; set; }
    public Guid TypeId { get; set; }
    public Guid VariableId { get; set; }

    public Variable(Guid dataTypeId, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be null or whitespace.", nameof(name));
        }
        if (dataTypeId == Guid.Empty)
        {
            throw new ArgumentException("Data type ID cannot be empty.", nameof(dataTypeId));
        }
        TypeId = dataTypeId;
        Name = name;
        VariableId = Guid.NewGuid();
    }
}
