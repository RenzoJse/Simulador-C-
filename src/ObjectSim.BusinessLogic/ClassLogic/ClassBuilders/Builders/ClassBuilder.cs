using ObjectSim.Domain;
using ObjectSim.IBusinessLogic;
using Attribute = ObjectSim.Domain.Attribute;

namespace ObjectSim.BusinessLogic.ClassLogic.ClassBuilders.Builders;

public class ClassBuilder(IMethodService methodService, IClassService classService, IAttributeService attributeService) : Builder(classService)
{
    private readonly IClassService _classService = classService;

    public override void SetAttributes(List<Attribute> attributes)
    {
        base.SetAttributes(attributes);

        List<Attribute> newAttributes = [];
        foreach(var attr in attributes)
        {
            var newAttribute = attributeService.CreateAttribute(attr);
            if(_classService.CanAddAttribute(Result, newAttribute))
            {
                newAttributes.Add(newAttribute);
            }
        }

        Result.Attributes = newAttributes;
    }

    public override void SetMethods(List<Method> methods)
    {
        base.SetMethods(methods);

        List<Method> newMethods = [];
        foreach(var method in methods)
        {
            var newMethod = methodService.Create(method);
            if(_classService.CanAddMethod(Result, newMethod))
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
