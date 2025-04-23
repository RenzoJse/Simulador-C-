using System.Text.RegularExpressions;

namespace ObjectSim.Domain;
public class LocalVariable
{
    public enum LocalVariableDataType
    {
        String,
        Char,
        Int,
        Decimal,
        Bool,
        DateTime
    }
    public Guid? Id { get; set; }

    #region Name
    private string? _name;
    public string? Name
    {
        get => _name;
        set
        {
            ValidateName(value!);
            _name = value;
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

        if(Regex.IsMatch(name, @"^\d"))
        {
            throw new ArgumentException("Name cannot be null or start with a num.");
        }
    }
    #endregion
    #region Type
    private LocalVariableDataType _type;
    public LocalVariableDataType Type
    {
        get => _type;
        set
        {
            ValidateDataType(value!);
            _type = value;
        }
    }
    private static void ValidateDataType(LocalVariableDataType type)
    {
        if(!Enum.IsDefined(typeof(LocalVariableDataType), type))
        {
            throw new ArgumentException("Invalid data type.");
        }
    }
    #endregion
}
