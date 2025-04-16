namespace ObjectSim.Domain;

public class Attribute
{
    public enum AttributeDataType
    {
        String,
        Char,
        Int,
        Decimal,
        Bool,
        DateTime
    }
    public enum AttributeVisibility
    {
        Public,
        Private,
        Protected,
        Internal,
        ProtectedInternal,
        PrivateProtected
    }
    public AttributeDataType DataType { get; set; }
    public AttributeVisibility Visibility { get; set; }
    public Guid Id { get; set; }
    public string? Name { get; set; } = null!;
    public void Validate()
    {
        ValidateId(Id);
        ValidateName(Name);
    }
    private static void ValidateId(Guid id)
    {
        if(id == Guid.Empty)
        {
            throw new ArgumentException("Id must be a valid non-empty GUID.");
        }
    }
    private static void ValidateName(string? name)
    {
        if(string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be null or whitespace.");
        }

        if(name.Length > 100)
        {
            throw new ArgumentException("Name cannot exceed 100 characters.");
        }
    }
}
