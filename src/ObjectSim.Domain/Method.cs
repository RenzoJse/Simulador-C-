using System.Text.RegularExpressions;

namespace ObjectSim.Domain;

public class Method
{
    public enum MethodAccessibility
    {
        Public,
        Private,
        Protected,
        Internal,
        ProtectedInternal,
        PrivateProtected
    }
    public Guid Id { get; init; } = Guid.NewGuid();

    public Guid? ClassId { get; set; }

    public Class? Class { get; set; }

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

    public DataType Type { get; set; } = null!;

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
    public List<DataType> Parameters { get; set; } = [];
    #endregion

    #region LocalVariable
    public List<DataType> LocalVariables { get; set; } = [];
    #endregion

    #region MethodsInvoke
    public List<Method> MethodsInvoke { get; set; } = [];
    #endregion

    #region Validations
    public void ValidateFields()
    {
        ValidateId(Id);
        ValidateName(Name);
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

    private static void ValidateAccesibility(MethodAccessibility accessibility)
    {
        if(!Enum.IsDefined(typeof(MethodAccessibility), accessibility))
        {
            throw new ArgumentException("Invalid accesibility type.");
        }
    }
    #endregion

}
