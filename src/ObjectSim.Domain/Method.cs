using System.Text.RegularExpressions;

namespace ObjectSim.Domain;

public class Method
{
    public enum MethodAccessibility
    {
        Public,
        Private,
        Protected,
    }
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid ClassId { get; set; }

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

    #region TypeId

    public Guid TypeId { get; set; }

    #endregion

    #region Abstract
    public bool Abstract { get; set; } = false;
    #endregion

    #region IsSealed
    public bool IsSealed { get; set; } = false;
    #endregion

    #region IsVirtual
    public bool IsVirtual { get; set; } = false;
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
    public List<Variable> Parameters { get; set; } = [];

    #endregion

    #region LocalVariable

    public List<Variable> LocalVariables { get; set; } = [];

    #endregion

    #region MethodsInvoke

    private List<InvokeMethod> _methodsInvoke = [];

    public List<InvokeMethod> MethodsInvoke
    {
        get => _methodsInvoke;
        init => _methodsInvoke = value ?? [];
    }

    public void CanAddInvokeMethod(Method method, Class classObj, string reference)
    {
        if(method == null)
        {
            throw new ArgumentNullException(nameof(method), "Method cannot be null.");
        }

        if(IsReservedReference(reference))
        {
            if(MethodIsNotInClass(method, classObj) && MethodIsNotFromParent(method, classObj))
            {
                throw new ArgumentException("The invoked method must be reachable from the current method.");
            }
        }
        else
        {
            if(MethodIsNotInAttributes(classObj, reference))
            {
                throw new ArgumentException("The invoked method must be reachable from the current method.");
            }
        }
    }

    private static bool IsReservedReference(string reference)
    {
        return reference is "this" or "base";
    }

    private static bool MethodIsNotInAttributes(Class classObj, string reference)
    {
        return classObj.Attributes!.Any(attribute => attribute.Name != reference);
    }

    private static bool MethodIsNotInClass(Method methods, Class classObj)
    {
        if(classObj.Methods!.Contains(methods))
        {
            return false;
        }

        return true;
    }

    private bool MethodIsNotFromParent(Method methods, Class classObj)
    {
        if(MethodClassHasParent(classObj))
        {
            var parentClass = classObj.Parent;
            if(parentClass!.Methods!.Contains(methods))
            {
                return false;
            }
        }

        return true;
    }

    private bool MethodClassHasParent(Class classObj)
    {
        return classObj.Parent != null;
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

        if(name.Length > 50)
        {
            throw new ArgumentException("Name cannot exceed 50 characters.");
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
