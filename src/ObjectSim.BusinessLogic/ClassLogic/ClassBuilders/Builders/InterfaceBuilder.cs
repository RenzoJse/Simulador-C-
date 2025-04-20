using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.BusinessLogic.ClassLogic.ClassBuilders.Builders;

public class InterfaceBuilder(IMethodService methodRepository,IClassService classService) : Builder(methodRepository, classService)
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
