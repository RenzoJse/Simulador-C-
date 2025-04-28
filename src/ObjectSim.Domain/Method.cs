using System.Text.RegularExpressions;

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
    public Guid Id { get; set; } = Guid.NewGuid();

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

    #endregion

    #region Type

    private MethodDataType _type;

    public MethodDataType Type
    {
        get => _type;
        set
        {
            ValidateDataType(value!);
            _type = value;
        }
    }
    #endregion

    #region Abstract
    public bool Abstract { get; set; } = false;
    #endregion

    #region IsSealed
    public bool IsSealed { get; set; } = false;
    #endregion

    #region IsOverride
    public bool IsOverride { get; set; } = false;
    #endregion 

    #region Accessibility

    private MethodAccessibility _accessibility;

    public MethodAccessibility Accessibility
    {
        get => _accessibility;
        set
        {
            ValidateAccesibility(value!);
            _accessibility = value;
        }
    }
    #endregion

    #region Parameters
    public List<Parameter> Parameters { get; set; } = [];
    #endregion

    #region LocalVariable
    public List<LocalVariable> LocalVariables { get; set; } = [];
    #endregion

    #region MethodsInvoke
    public List<Method> MethodsInvoke { get; set; } = [];
    #endregion

    #region Validations
    public void ValidateFields()
    {
        ValidateId(Id);
        ValidateName(Name);
        ValidateDataType(Type);
        ValidateAccesibility(Accessibility);
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

        if(Regex.IsMatch(name, @"^\d"))
        {
            throw new ArgumentException("Name cannot be null or start with a num.");
        }
    }
    private static void ValidateDataType(MethodDataType type)
    {
        if(!Enum.IsDefined(typeof(MethodDataType), type))
        {
            throw new ArgumentException("Invalid data type.");
        }
    }
    private static void ValidateAccesibility(MethodAccessibility accessibility)
    {
        if(!Enum.IsDefined(typeof(MethodAccessibility), accessibility))
        {
            throw new ArgumentException("Invalid accesibility type.");
        }
    }
    #endregion
}
