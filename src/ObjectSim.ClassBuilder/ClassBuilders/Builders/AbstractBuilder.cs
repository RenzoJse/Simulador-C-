using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;
using Attribute = ObjectSim.Domain.Attribute;

namespace ObjectSim.ClassConstructor.ClassBuilders.Builders;

public class AbstractBuilder(IMethodServiceCreate methodService, IAttributeService attributeService) : Builder()
{
    public override void SetAttributes(List<CreateAttributeArgs> attributes)
    {
        base.SetAttributes(attributes);

        List<Attribute> newAttributes = [];
        foreach(var attr in attributes)
        {
            try
            {
                Attribute newAttribute = attributeService.CreateAttribute(attr);
                if(Class.CanAddAttribute(Result, newAttribute))
                {
                    newAttributes.Add(newAttribute);
                }
            }
            catch
            {
            }
        }

        Result.Attributes = newAttributes;
    }

    public override void SetMethods(List<CreateMethodArgs> methods)
    {
        base.SetMethods(methods);

        List<Method> newMethods = [];
        foreach(var method in methods)
        {
            try
            {
                method.IsAbstract = true;
                var newMethod = methodService.CreateMethod(method);
                if(Class.CanAddMethod(Result, newMethod))
                {
                    newMethods.Add(newMethod);
                }
            }
            catch
            {
            }
        }

        var parent = Result.Parent;
        if(parent is not null && (bool)parent.IsInterface!)
        {
            if (parent.Methods!.Select(parentMethod => newMethods.Any(m => m.Name == parentMethod.Name)).Any(isImplemented => !isImplemented))
            {
                throw new ArgumentException("Parent class is an interface. Should implement all his methods");
            }
        }

        Result.Methods = newMethods;
    }
}
