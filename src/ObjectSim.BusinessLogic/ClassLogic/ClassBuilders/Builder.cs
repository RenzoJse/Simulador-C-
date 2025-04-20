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
        DoesParentExist(parent);
        Result.Parent = parent;
    }

    private static void DoesParentExist(Class parent)
    {
        if (parent is null)
        {
            throw new ArgumentException("Parent does not exist");
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

    public virtual void SetAttributes(List<Guid> attributes)
    {
        ArgumentNullException.ThrowIfNull(attributes);
    }

    public virtual void SetMethods(List<Guid> methods)
    {
        ArgumentNullException.ThrowIfNull(methods);
    }

    public Class GetResult()
    {
        methodService.GetById(Result.Id);
        return Result;
    }

}
