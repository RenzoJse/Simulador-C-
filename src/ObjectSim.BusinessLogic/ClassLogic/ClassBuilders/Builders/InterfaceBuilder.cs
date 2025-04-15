using ObjectSim.Domain;
using Attribute = ObjectSim.Domain.Attribute;

namespace ObjectSim.BusinessLogic.ClassLogic.ClassBuilders.Builders;

public class InterfaceBuilder : Builder
{
    public override void SetAttributes(List<Guid> attributes)
    {
        base.SetAttributes(attributes);
    }

    public override void SetMethods(List<Guid> methods)
    {
        base.SetMethods(methods);
    }
}
