using ObjectSim.Domain;
using ObjectSim.IBusinessLogic;
using Attribute = ObjectSim.Domain.Attribute;

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
        IsParentSealed(parent);
        Result.Parent = parent;
    }

    private static void DoesParentExist(Class parent)
    {
        if (parent is null)
        {
            throw new ArgumentException("Parent does not exist");
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
        else
        {
            foreach (var attr in attributes.Select(attributeService.Create))
            {
                try
                {
                    var newAttribute = attributeService.Create(attr);
                    classService.AddAttribute(Result.Id, newAttribute);
                }
                catch(ArgumentException)
                {
                    continue;
                }
            }
        }
    }

    public virtual void SetMethods(List<Method> methods)
    {
        ArgumentNullException.ThrowIfNull(methods);

        if(methods.Count == 0)
        {
            Result.Methods = [];
        }
        else
        {
            foreach (var method in methods.Select(methodService.Create))
            {
                try
                {
                    var newMethod = methodService.Create(method);
                    classService.AddMethod(Result.Id, newMethod);
                }
                catch(ArgumentException)
                {
                    continue;
                }
            }
        }
    }

    public Class GetResult()
    {
        return Result;
    }

}
