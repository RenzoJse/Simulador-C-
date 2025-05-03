using ObjectSim.Domain.Args;

namespace ObjectSim.ClassConstructor.ClassBuilders.Builders;

public class InterfaceBuilder() : Builder()
{
    public override void SetAttributes(List<CreateAttributeArgs> attributes)
    {
        base.SetAttributes(attributes);
        Result.Attributes = [];
    }

    public override void SetMethods(List<CreateMethodArgs> methods)
    {
        base.SetMethods(methods);
        Result.Methods = [];
    }

    public override void SetAbstraction(bool? abstraction)
    {
        Result.IsAbstract = true;
    }
}
