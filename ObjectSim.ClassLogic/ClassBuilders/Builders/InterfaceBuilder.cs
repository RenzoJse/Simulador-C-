using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.ClassLogic.ClassBuilders.Builders;

public class InterfaceBuilder(IClassService classService, IAttributeService attributeService) : Builder(classService, attributeService)
{
    public override void SetAttributes(List<CreateAttributeArgs> attributes)
    {
        base.SetAttributes(attributes);
    }

    public override void SetMethods(List<CreateMethodArgs> methods)
    {
        base.SetMethods(methods);
    }
}
