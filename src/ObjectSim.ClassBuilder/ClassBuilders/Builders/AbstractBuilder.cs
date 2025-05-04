using ObjectSim.Domain.Args;

namespace ObjectSim.ClassConstructor.ClassBuilders.Builders;

public class AbstractBuilder() : Builder()
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
