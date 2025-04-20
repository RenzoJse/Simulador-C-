using ObjectSim.IBusinessLogic;

namespace ObjectSim.BusinessLogic.ClassLogic.ClassBuilders.Builders;

public class AbstractBuilder(IMethodService methodService,IClassService classService, IAttributeService attributeService) : Builder(methodService, classService, attributeService)
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
