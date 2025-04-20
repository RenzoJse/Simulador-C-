using ObjectSim.IBusinessLogic;

namespace ObjectSim.BusinessLogic.ClassLogic.ClassBuilders.Builders;

public class AbstractBuilder(IMethodService methodService,IClassService classService) : Builder(methodService, classService)
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
