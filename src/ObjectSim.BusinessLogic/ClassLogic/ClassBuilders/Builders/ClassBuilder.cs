using ObjectSim.Domain;
using ObjectSim.IBusinessLogic;
using Attribute = ObjectSim.Domain.Attribute;

namespace ObjectSim.BusinessLogic.ClassLogic.ClassBuilders.Builders;

public class ClassBuilder(IMethodService methodService, IClassService classService, IAttributeService attributeService) : Builder(classService)
{
    public override void SetAttributes(List<Attribute> attributes)
    {
        base.SetAttributes(attributes);

        List<Attribute> newAttributes = [];
        foreach(var attr in attributes)
        {
            var newAttribute = attributeService.Create(attr);
            if(classService.CanAddAttribute(Result, newAttribute))
            {
                newAttributes.Add(newAttribute);
            }
        }

        Result.Attributes = newAttributes;
    }

    public override void SetMethods(List<Method> methods)
    {
        base.SetMethods(methods);
        foreach(var method in methods.Select(methodService.Create))
        {
            try
            {
                var newMethod = methodService.Create(method);
                classService.AddMethod(Result.Id, newMethod);
            }
            catch(ArgumentException)
            {
            }
        }
    }
}
