using ObjectSim.Domain;

namespace ObjectSim.BusinessLogic.ClassesBuilders;

public abstract class Builder
{
    private const int MaxNameLength = 15;
    private const int MinNameLength = 3;

    public virtual void SetName(string name)
    {
        IsValidName(name);
    }

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

    public virtual void SetParent(Class parent)
    {
        ArgumentNullException.ThrowIfNull(parent);
        if(parent.IsSealed)
        {
            throw new ArgumentException("Parent class cannot be sealed");
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
}
