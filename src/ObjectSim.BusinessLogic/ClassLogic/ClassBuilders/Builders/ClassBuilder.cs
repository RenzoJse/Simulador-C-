using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;
using Attribute = ObjectSim.Domain.Attribute;

namespace ObjectSim.BusinessLogic.ClassLogic.ClassBuilders.Builders;

public class ClassBuilder(IMethodService methodService, IClassService classService, IAttributeService attributeService) : Builder(classService)
{
    private readonly IClassService _classService = classService;

    public override void SetAttributes(List<CreateAttributeArgs> attributes)
    {
        base.SetAttributes(attributes);

        List<Attribute> newAttributes = [];
        newAttributes.AddRange(attributes.Select(attributeService.CreateAttribute).Where(newAttribute => _classService.CanAddAttribute(Result, newAttribute)));

        Result.Attributes = newAttributes;
    }

    public override void SetMethods(List<Method> methods)
    {
        base.SetMethods(methods);

        List<Method> newMethods = [];
        newMethods.AddRange(methods.Select(methodService.Create).Where(newMethod => _classService.CanAddMethod(Result, newMethod)));

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
