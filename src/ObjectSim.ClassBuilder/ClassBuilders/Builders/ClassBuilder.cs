using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;
using Attribute = ObjectSim.Domain.Attribute;

namespace ObjectSim.ClassConstructor.ClassBuilders.Builders;

public class ClassBuilder(IMethodServiceCreate methodService, IAttributeService attributeService) : Builder()
{
    public override void SetAttributes(List<CreateAttributeArgs> attributes)
    {
        base.SetAttributes(attributes);

        List<Attribute> newAttributes = [];
        foreach(var attr in attributes)
        {
            try
            {
                var newAttribute = attributeService.CreateAttribute(attr);
                if(Result.CanAddAttribute(newAttribute))
                {
                    newAttributes.Add(newAttribute);
                }
            }
            catch
            {
                //ignored to build if it is not possible to create attribute
            }
        }

        Result.Attributes = newAttributes;
    }

    public override void SetMethods(List<CreateMethodArgs> methods)
    {
        base.SetMethods(methods);
        var newMethods = BuildValidMethods(methods);
        ValidateInterfaceImplementation(newMethods);
        Result.Methods = newMethods;
    }

    private List<Method> BuildValidMethods(List<CreateMethodArgs> methods)
    {
        var validMethods = new List<Method>();
        foreach(var method in methods)
        {
            try
            {
                var newMethod = methodService.CreateMethod(method);
                if(Result.CanAddMethod(newMethod))
                {
                    validMethods.Add(newMethod);
                }
            }
            catch
            {
                //ignored to build if it is not possible to create method
            }
        }
        return validMethods;
    }

    private void ValidateInterfaceImplementation(List<Method> methods)
    {
        var parent = Result.Parent;
        if(parent is not null && parent.IsInterface == true)
        {
            if((from parentMethod in parent.Methods ?? Enumerable.Empty<Method>() select methods.Any(m => m.Name == parentMethod.Name)).Any(isImplemented => !isImplemented))
            {
                throw new ArgumentException("Parent class is an interface. Should implement all its methods");
            }
        }
    }
}
