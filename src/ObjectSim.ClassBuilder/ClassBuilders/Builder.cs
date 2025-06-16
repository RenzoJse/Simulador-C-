using ObjectSim.Domain;
using ObjectSim.Domain.Args;

namespace ObjectSim.ClassConstructor.ClassBuilders;

public abstract class Builder()
{
    protected Class Result { get; } = new Class() { Attributes = [], Methods = [] };

    public virtual void SetName(string name)
    {
        Result.Name = name;
    }

    public virtual void SetParent(Class? parent)
    {
        Result.Parent = null;

        if(parent != null)
        {
            IsParentSealed(parent);
            Result.Parent = parent;
        }
    }

    private static void IsParentSealed(Class parent)
    {
        if((bool)parent.IsSealed!)
        {
            throw new ArgumentException("Cant have a sealed class as parent");
        }
    }

    public virtual void SetAbstraction(bool? abstraction)
    {
        Result.IsAbstract = abstraction;
    }

    public virtual void SetInterface(bool? isInterface)
    {
        Result.IsInterface = isInterface;
    }

    public virtual void SetSealed(bool? sealedClass)
    {
        Result.IsSealed = sealedClass;
    }

    public virtual void SetAttributes(List<CreateAttributeArgs> attributes)
    {
        ArgumentNullException.ThrowIfNull(attributes);

        if(attributes.Count == 0)
        {
            Result.Attributes = [];
        }
    }

    public virtual void SetMethods(List<CreateMethodArgs> methods)
    {
        ArgumentNullException.ThrowIfNull(methods);

        if(methods.Count == 0 && ParentIsInterfaceWithUnimplementedMethods())
        {
            throw new ArgumentException("Parent class is an interface and has methods that are not implemented");
        }

        Result.Methods = [];
    }

    private bool ParentIsInterfaceWithUnimplementedMethods()
    {
        var parent = Result.Parent;
        return parent is not null && (bool)parent.IsInterface! && parent.Methods!.Count > 0;
    }

    public Class GetResult()
    {
        return Result;
    }

}
