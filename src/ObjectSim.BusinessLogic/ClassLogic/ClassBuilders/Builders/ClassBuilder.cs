using ObjectSim.Domain;
using Attribute = ObjectSim.Domain.Attribute;

namespace ObjectSim.BusinessLogic.ClassLogic.ClassBuilders.Builders;

public class ClassBuilder : Builder
{
    public override void SetAttributes(List<Attribute> attributes)
    {
        base.SetAttributes(attributes);
    }

    public override void SetMethods(List<Method> methods)
    {
        base.SetMethods(methods);
    }
}
