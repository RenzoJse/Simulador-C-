using ObjectSim.Domain;
using Attribute = ObjectSim.Domain.Attribute;

namespace ClassManagement.ClassesBuilders.Builders;

public class AbstractBuilder : Builder
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
