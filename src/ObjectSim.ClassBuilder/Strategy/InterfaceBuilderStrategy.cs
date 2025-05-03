using ObjectSim.ClassConstructor.ClassBuilders;
using ObjectSim.ClassConstructor.ClassBuilders.Builders;
using ObjectSim.Domain.Args;

namespace ObjectSim.ClassConstructor.Strategy;

public class InterfaceBuilderStrategy() : IBuilderStrategy
{
    public bool WhichIsMyBuilder(CreateClassArgs args)
    {
        return args.IsInterface == true;
    }

    public Builder CreateBuilder()
    {
        return new InterfaceBuilder();
    }
}
