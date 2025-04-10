using ObjectSim.Domain;
using Attribute = ObjectSim.Domain.Attribute;

namespace ClassManagement.ClassesBuilders;

public abstract class Builder
{
    private const int MaxNameLength = 15;
    private const int MinNameLength = 3;
    public Class Result { get; private set; } = new Class();

    #region SetName

    public virtual void SetName(string name)
    {
        IsValidName(name);
        Result.Name = name;
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

    public virtual void SetAbstraction(bool? abstraction)
    {
        ArgumentNullException.ThrowIfNull(abstraction);
    }

    public virtual void SetSealed(bool? sealedClass)
    {
        ArgumentNullException.ThrowIfNull(sealedClass);
    }

    public virtual void SetAttributes(List<Attribute> attributes)
    {
        ArgumentNullException.ThrowIfNull(attributes);
    }

    public virtual void SetMethods(List<Method> methods)
    {
        ArgumentNullException.ThrowIfNull(methods);
    }

    #region SetParent

    public virtual void SetParent(Class parent)
    {
        ArgumentNullException.ThrowIfNull(parent);
        IsParentSealed(parent);
    }

    private static void IsParentSealed(Class parent)
    {
        if(parent.IsSealed)
        {
            throw new ArgumentException("Parent class cannot be sealed");
        }
    }

    #endregion

}
