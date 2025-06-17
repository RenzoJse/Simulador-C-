using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;
using Attribute = ObjectSim.Domain.Attribute;

namespace ObjectSim.ClassConstructor.ClassBuilders.Builders;

public class AbstractBuilder(IMethodServiceCreate methodService, IAttributeService attributeService) : Builder
{
    public override void SetAttributes(List<CreateAttributeArgs> attributes)
    {
        base.SetAttributes(attributes);

        var validAttributes = attributes.Select(CreateAttributes).OfType<Attribute>().Where(attribute => Result.CanAddAttribute(attribute)).ToList();

        Result.Attributes = validAttributes;
    }

    private Attribute? CreateAttributes(CreateAttributeArgs args)
    {
        try
        {
            var attribute = attributeService.BuilderCreateAttribute(args);
            ValidateAttributeIsStatic(attribute);
            return attribute;
        }
        catch
        {
            return null;
        }
    }

    private static void ValidateAttributeIsStatic(Attribute attribute)
    {
        if (attribute.IsStatic)
        {
            throw new ArgumentException("Attributes in abstract class cannot be static");
        }
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
                var newMethod = methodService.BuilderCreateMethod(method);
                if(Result.CanAddMethod(newMethod))
                {
                    newMethods.Add(newMethod);
                }
            }
            catch
            {
                // ignored
            }
        }

        var parent = Result.Parent;
        if(parent is not null && (bool)parent.IsInterface!)
        {
            if(parent.Methods!.Select(parentMethod => newMethods.Any(m => m.Name == parentMethod.Name)).Any(isImplemented => !isImplemented))
            {
                throw new ArgumentException("Parent class is an interface. Should implement all his methods");
            }
        }

        Result.Methods = newMethods;
    }
}
