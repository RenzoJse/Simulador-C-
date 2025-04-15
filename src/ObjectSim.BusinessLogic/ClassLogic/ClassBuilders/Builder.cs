using ObjectSim.Domain;

namespace ObjectSim.BusinessLogic.ClassLogic.ClassBuilders;

public abstract class Builder
{
    private Class Result { get; } = new Class();

    public virtual void SetName(string name)
    {
        Result.Name = name;
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

    public virtual void SetParent(Guid idParent)
    {
        ArgumentNullException.ThrowIfNull(idParent);
    }

    public Class GetResult()
    {
        return Result;
    }

}
