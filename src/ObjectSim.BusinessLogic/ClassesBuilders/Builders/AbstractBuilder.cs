using ObjectSim.Domain;

namespace ObjectSim.BusinessLogic.ClassesBuilders.Builders;

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

    public override Class GetResult()
    {
        return null;
    }
}
