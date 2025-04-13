namespace ObjectSim.Domain;

public class Class
{
    public Guid Id { get; init; } = Guid.NewGuid();

    #region Name

    private string? _name;
    private const int MaxNameLength = 15;
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
            throw new ArgumentException("Name cannot be longer than 15 characters");
        }
    }

    private static void VerifyMinNameLenght(string name)
    {
        if(name.Length < MinNameLength)
        {
            throw new ArgumentException("Name cannot be shorter than 3 characters");
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
            _attributes = value.ToList();
        }
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
            _methods = value.ToList();
        }
    }

    #endregion

    #region Parent

    private Class? _parent;

    public Class? Parent
    {
        get => _parent;
        set
        {
            if (value != null)
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
