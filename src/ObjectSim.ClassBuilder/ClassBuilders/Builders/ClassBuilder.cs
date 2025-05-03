using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;
using Attribute = ObjectSim.Domain.Attribute;

namespace ObjectSim.ClassLogic.ClassBuilders.Builders;

public class ClassBuilder(IMethodServiceCreate methodService, IAttributeService attributeService) : Builder()
{
    public override void SetAttributes(List<CreateAttributeArgs> attributes)
    {
        base.SetAttributes(attributes);

        List<Attribute> newAttributes = [];
        foreach(var attr in attributes)
        {
            var newAttribute = attributeService.CreateAttribute(attr);
            if(Result.CanAddAttribute(Result, newAttribute))
            {
                newAttributes.Add(newAttribute);
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
            var newMethod = methodService.CreateMethod(method);
            if(Result.CanAddMethod(Result, newMethod))
            {
                newMethods.Add(newMethod);
            }
        }

        var parent = Result.Parent;
        if(parent is not null && (bool)parent.IsInterface!)
        {
            foreach(var parentMethod in parent.Methods!)
            {
                var isImplemented = newMethods.Any(m => m.Name == parentMethod.Name);
                if(!isImplemented)
                {
                    throw new ArgumentException("Parent class is an interface. Should implement all his methods");
                }
            }
        }

        Result.Methods = newMethods;
    }
}
