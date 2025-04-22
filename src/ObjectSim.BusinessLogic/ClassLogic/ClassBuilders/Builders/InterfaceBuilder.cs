using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain;
using ObjectSim.IBusinessLogic;
using Attribute = ObjectSim.Domain.Attribute;

namespace ObjectSim.BusinessLogic.ClassLogic.ClassBuilders.Builders;

public class InterfaceBuilder(IClassService classService) : Builder(classService)
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
