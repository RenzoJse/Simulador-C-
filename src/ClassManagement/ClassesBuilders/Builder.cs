using ObjectSim.Domain;
using Attribute = ObjectSim.Domain.Attribute;

namespace ClassManagement.ClassesBuilders;

public abstract class Builder
{
    private Class Result { get; set; } = new Class();

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

    public virtual void SetAttributes(List<Attribute> attributes)
    {
       Result.Attributes = attributes;
    }

    public virtual void SetMethods(List<Method> methods)
    {
        Result.Methods = methods;
    }

    public virtual void SetParent(Class parent)
    {
        Result.Parent = parent;
    }

}
