using ObjectSim.Domain;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.BusinessLogic.ClassLogic.ClassBuilders;

public abstract class Builder(IMethodService methodService, IClassService classService, IAttributeService attributeService)
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
        var newAttributes = new List<Domain.Attribute>();
        foreach (var attr in attributes.Select(attributeService.GetById))
        {
            if (attr is null)
            {
                throw new ArgumentException("Attribute does not exist");
            }

            if(Result.Parent is not null)
            {
                var parentAttr = Result.Parent;
                foreach (var attribute in parentAttr.Attributes!)
                {
                    if (attribute.Id == attr.Id)
                    {
                       throw new ArgumentException("Attribute already exists in parent class");
                    }

                    if(attribute.Name == attr.Name)
                    {
                        throw new ArgumentException("Attribute name already exists in parent class");
                    }
                }
            }
            newAttributes.Add(attr);
        }
        Result.Attributes = newAttributes;
    }

    public virtual void SetMethods(List<Guid> methods)
    {
        methodService.GetById(methods.First());
        ArgumentNullException.ThrowIfNull(methods);
    }

    public Class GetResult()
    {
        return Result;
    }

}
