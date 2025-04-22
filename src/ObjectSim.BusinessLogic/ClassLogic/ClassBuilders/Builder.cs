using Azure.Core;
using ObjectSim.Domain;
using ObjectSim.IBusinessLogic;
using Attribute = ObjectSim.Domain.Attribute;

namespace ObjectSim.BusinessLogic.ClassLogic.ClassBuilders;

public abstract class Builder(IClassService classService)
{
    protected Class Result { get; } = new Class();

    public virtual void SetName(string name)
    {
        Result.Name = name;
    }

    public virtual void SetParent(Guid? idParent)
    {
        Class parent = null!;
        try
        {
             parent = classService.GetById(idParent);
        }
        catch(ArgumentException)
        {
            Result.Parent = null;
        }

        if(parent != null)
        {
            IsParentSealed(parent);
            Result.Parent = parent;
        }
    }

    private static void IsParentSealed(Class parent)
    {
        if ((bool)parent.IsSealed!)
        {
            throw new ArgumentException("Cant have a sealed class as parent");
        }
    }

    public virtual void SetAbstraction(bool? abstraction)
    {
        Result.IsAbstract = abstraction;
    }

    public virtual void SetSealed(bool? sealedClass)
    {
        Result.IsSealed = sealedClass;
    }

    public virtual void SetAttributes(List<Attribute> attributes)
    {
        ArgumentNullException.ThrowIfNull(attributes);

        if(attributes.Count == 0)
        {
            Result.Attributes = [];
        }
    }

    public virtual void SetMethods(List<Method> methods)
    {
        ArgumentNullException.ThrowIfNull(methods);

        if(methods.Count == 0)
        {
            if(Result.Parent is not null)
            {
                if((bool)Result.Parent.IsInterface!)
                {
                    throw new ArgumentException("Parent class is an interface and has methods that are not implemented");
                }
            }
            Result.Methods = [];
        }
    }

    public Class GetResult()
    {
        return Result;
    }

}
