using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.BusinessLogic.ClassLogic.ClassBuilders.Builders;

public class InterfaceBuilder(IClassService classService) : Builder(classService)
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
