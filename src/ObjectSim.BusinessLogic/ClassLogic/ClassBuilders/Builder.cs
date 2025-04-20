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
            ValidateAttributeAgainstParentIfNeeded(attr);
            newAttributes.Add(attr);
        }
        Result.Attributes = newAttributes;
    }

    private void ValidateAttributeAgainstParentIfNeeded(Domain.Attribute attribute)
    {
        if (Result.Parent is null || Result.Parent.Attributes!.Count == 0)
        {
            return;
        }

        ValidateAttributeAgainstParent(Result.Parent, attribute);
    }

    private static void ValidateAttributeAgainstParent(Class parent, Domain.Attribute attribute)
    {
        foreach (var parentAttribute in parent.Attributes!)
        {
            ValidateParentDoesNotHaveAttribute(parentAttribute.Id, attribute.Id);
            ValidateParentDoesNotHaveSameAttributeName(parentAttribute.Name!, attribute.Name!);
        }
    }

    private static void ValidateParentDoesNotHaveAttribute(Guid parentAttributeId, Guid attributeId)
    {
        if(parentAttributeId == attributeId)
        {
            throw new ArgumentException("Attribute already exists in parent class");
        }
    }

    private static void ValidateParentDoesNotHaveSameAttributeName(string parentAttributeName, string attributeName)
    {
        if (parentAttributeName == attributeName)
        {
            throw new ArgumentException("Attribute name already exists in parent class");
        }
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
