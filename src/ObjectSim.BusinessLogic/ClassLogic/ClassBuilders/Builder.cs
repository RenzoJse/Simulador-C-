using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.BusinessLogic.ClassLogic.ClassBuilders;

public abstract class Builder(IMethodService methodService, IClassService classService)
{
    private Class Result { get; } = new Class();

    public virtual void SetName(string name)
    {
        Result.Name = name;
    }

    public virtual void SetParent(Guid idParent)
    {
        var parent = classService.GetById(idParent);
        if (parent is null)
        {
            throw new ArgumentException("Parent does not exist");
        }

        Result.Parent = parent;
    }

    public virtual void SetAbstraction(bool? abstraction)
    {
        Result.IsAbstract = abstraction;
    }

    public virtual void SetSealed(bool? sealedClass)
    {
        Result.IsSealed = sealedClass;
    }

    public virtual void SetAttributes(List<Guid> attributes)
    {
        ArgumentNullException.ThrowIfNull(attributes);
    }

    public virtual void SetMethods(List<Guid> methods)
    {
        ArgumentNullException.ThrowIfNull(methods);
        var parentMethodsList = new List<Method>();
        var parent = Result.Parent;
        if(Result.Parent is not null)
        {
            parentMethodsList = parent!.Methods;
        }
        foreach (var method in methods.Select(methodService.GetById))
        {
            if (method is null)
            {
                throw new ArgumentException("Method does not exist");
            }

            if(parent is not null && (bool)parent.IsInterface!)
            {
                if(parentMethodsList!.Contains(method)!)
                {
                    throw new ArgumentException("Should implement interface methods");
                }
            }
            if (parentMethodsList!.Contains(method))
            {
                throw new ArgumentException("Method already exists in parent");
            }
        }
    }

    public Class GetResult()
    {
        return Result;
    }

}
