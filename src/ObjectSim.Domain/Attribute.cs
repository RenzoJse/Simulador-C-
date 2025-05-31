using System.Text.RegularExpressions;

namespace ObjectSim.Domain;

public class Attribute
{
    public enum AttributeVisibility
    {
        Public,
        Private,
        Protected,
        Internal
    }
    public DataType DataType { get; set; } = null!;
    public AttributeVisibility Visibility { get; set; }
    public Guid Id { get; set; }
    public Guid ClassId { get; set; }
    public string? Name { get; set; } = null!;
    public void Validate()
    {
        ValidateId(Id);
        ValidateId(ClassId);
        ValidateName(Name);
        ValidateDataType(DataType);
        ValidateVisibility(Visibility);
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

        if(name.Length > 10 || name.Length < 1)
        {
            throw new ArgumentException("Name cannot be less than 1 or more than 10 characters.");
        }
        if(Regex.IsMatch(name, @"^\d"))
        {
            throw new ArgumentException("Name cannot be null or start with a num.");
        }

    }
    public static void ValidateDataType(DataType DataType)
    {
        if(DataType == null)
        {
            throw new ArgumentException("DataType is required.");
        }
    }
    private static void ValidateVisibility(AttributeVisibility visibility)
    {
        if(!Enum.IsDefined(typeof(AttributeVisibility), visibility))
        {
            throw new ArgumentException("Invalid visibility type.");
        }
    }
}
