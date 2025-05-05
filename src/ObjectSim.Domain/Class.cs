namespace ObjectSim.Domain;

public class Class
{
    public Guid Id { get; init; } = Guid.NewGuid();

    #region Name

    private string? _name;
    private const int MaxNameLength = 20;
    private const int MinNameLength = 3;

    public string? Name
    {
        get => _name;
        set
        {
            IsValidName(value!);
            _name = value;
        }
    }

    private static void IsValidName(string name)
    {
        ArgumentNullException.ThrowIfNull(name);
        VerifyNameLenght(name);
    }

    private static void VerifyNameLenght(string name)
    {
        VerifyMaxNameLenght(name);
        VerifyMinNameLenght(name);
    }

    private static void VerifyMaxNameLenght(string name)
    {
        if(name.Length > MaxNameLength)
        {
            throw new ArgumentException("Name cannot be longer than " + MaxNameLength + " characters");
        }
    }

    private static void VerifyMinNameLenght(string name)
    {
        if(name.Length < MinNameLength)
        {
            throw new ArgumentException("Name cannot be shorter than " + MinNameLength + " characters");
        }
    }

    #endregion

    #region Abstraction

    private bool? _isAbstract;

    public bool? IsAbstract
    {
        get => _isAbstract;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            _isAbstract = value;
        }
    }

    #endregion

    #region Interface

    private bool? _isInterface;

    public bool? IsInterface
    {
        get => _isInterface;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            _isInterface = value;
        }
    }

    #endregion

    #region Sealed

    private bool? _isSealed;

    public bool? IsSealed
    {
        get => _isSealed;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            _isSealed = value;
        }
    }

    #endregion

    #region Attributes

    private List<Attribute>? _attributes;

    public List<Attribute>? Attributes
    {
        get => _attributes;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            _attributes = IsInterface == true ? [] : value.ToList();
        }
    }

    public void AddAttribute(Attribute attribute)
    {
        if(CanAddAttribute(attribute))
        {
            Attributes!.Add(attribute);
        }
    }

    public bool CanAddAttribute(Attribute attribute)
    {
        if(IsInterface == true)
        {
            throw new ArgumentException("Cannot add attribute to an interface.");
        }

        if(Attributes!.Any(classAttribute => classAttribute.Name == attribute.Name))
        {
            throw new ArgumentException("Attribute name already exists in class.");
        }

        return true;
    }

    #endregion

    #region Methods

    private List<Method>? _methods;

    public List<Method>? Methods
    {
        get => _methods;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            if(IsInterface == true)
            {
                if(value.Any(method => method.Abstract != true))
                {
                    throw new ArgumentException("Methods in an interface must be abstract");
                }
            }

            _methods = value.ToList();
        }
    }

    public bool CanAddMethod(Method method)
    {
        ValidateMethodUniqueness(method);

        if(IsInterface == true)
        {
            ValidateInterfaceMethodConstraints(method);
        }

        return true;
    }

    public void ValidateMethodUniqueness(Method method)
    {
        if(Methods!.Any(classMethod =>
               classMethod.Name == method.Name &&
               classMethod.Type.IsSameType(method.Type) &&
               method.IsOverride == false &&
               AreParametersEqual(classMethod.Parameters, method.Parameters)))
        {
            throw new ArgumentException("Method already exists in class.");
        }
    }

    private static void ValidateInterfaceMethodConstraints(Method method)
    {
        if(method.IsSealed)
        {
            throw new ArgumentException("Method cannot be sealed in an interface.");
        }
        if(method.IsOverride)
        {
            throw new ArgumentException("Method cannot be override in an interface.");
        }
        if(method.Accessibility == Method.MethodAccessibility.Private)
        {
            throw new ArgumentException("Method cannot be private in an interface.");
        }
        if(method.LocalVariables.Count > 0)
        {
            throw new ArgumentException("Method cannot have local variables in an interface.");
        }
        if(method.MethodsInvoke.Count > 0)
        {
            throw new ArgumentException("Method cannot invoke other methods in an interface.");
        }
        if(!method.Abstract)
        {
            method.Abstract = true;
        }
    }

    private static bool AreParametersEqual(List<DataType> parameters1, List<DataType> parameters2)
    {
        if(parameters1.Count != parameters2.Count)
        {
            return false;
        }

        for(var i = 0; i < parameters1.Count; i++)
        {
            var p1 = parameters1[i];
            var p2 = parameters2[i];

            if(p1.Name != p2.Name || p1.Type != p2.Type)
            {
                return false;
            }
        }

        return true;
    }

    #endregion

    #region Parent

    private Class? _parent;

    public Class? Parent
    {
        get => _parent;
        set
        {
            if(value != null)
            {
                VerifyParent(value);
            }

            _parent = value;
        }
    }

    private static void VerifyParent(Class parent)
    {
        IsParentSealed(parent);
    }

    private static void IsParentSealed(Class parent)
    {
        if(parent.IsSealed == true)
        {
            throw new ArgumentException("Parent class cannot be sealed");
        }
    }

    #endregion
}
