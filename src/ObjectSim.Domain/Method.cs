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

    public Guid? ClassId { get; set; }

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

    public Guid TypeId { get; set; }
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

    private List<InvokeMethod> _methodsInvoke = [];

    public List<InvokeMethod> MethodsInvoke
    {
        get => _methodsInvoke;
        init => _methodsInvoke = value ?? [];
    }

    public void CanAddInvokeMethod(Method method, Class classObj, string reference)
    {
        if (method == null)
        {
            throw new ArgumentNullException(nameof(method), "Method cannot be null.");
        }

        if (MethodIsNotInClass(method, classObj)
            && MethodIsNotFromParent(method, classObj)
            && MethodIsNotInAttributes(method, classObj)
            && MethodIsNotInLocalVariable(method)
            && MethodIsNotInParameters(method))
        {
            throw new ArgumentException("The invoked method must be reachable from the current method.");
        }
        
    }

    private static bool MethodIsNotInParameters(Method method)
    {
        var parameters = method.Parameters;
        return parameters.All(parameter => !parameter.MethodIds.Contains(method.Id));
    }

    private static bool MethodIsNotInLocalVariable(Method method)
    {
        var localVariables = method.LocalVariables;
        return localVariables.All(localVariable => !localVariable.MethodIds.Contains(method.Id));
    }

    private static bool MethodIsNotInAttributes(Method methods, Class classObj)
    {
        return classObj.Attributes!.Select(attribute => attribute.DataType).All(attributeDataType => !attributeDataType.MethodIds.Contains(methods.Id));
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
        if (MethodClassHasParent(classObj))
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
