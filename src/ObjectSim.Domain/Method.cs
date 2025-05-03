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

    public string GetTypeString() => Type?.Type ?? string.Empty;

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

    private List<Method> _methodsInvoke = [];

    public List<Method> MethodsInvoke
    {
        get => _methodsInvoke;
        set
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value), "InvokeMethod cannot be null.");
            }

            if (MethodIsNotInClass(value) && MethodIsNotFromParent(value))
            {
                throw new ArgumentException("The invoked method must be reachable from the current method.");
            }

            _methodsInvoke = value;
        }
    }

    private bool MethodIsNotInClass(List<Method> methods)
    {
        return methods.Any(method => method.Class != Class);
    }

    private bool MethodClassHasParent()
    {
        return Class?.Parent != null;
    }

    private bool MethodIsNotFromParent(List<Method> methods)
    {
        if (MethodClassHasParent())
        {
            var parentClass = Class!.Parent;
            foreach(var method in parentClass!.Methods!)
            {
                if(methods.Contains(method))
                {
                    return false;
                }
            }
        }

        return true;
    }

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
