using ObjectSim.ClassLogic.ClassBuilders;
using ObjectSim.ClassLogic.ClassBuilders.Builders;
using ObjectSim.Domain.Args;

namespace ObjectSim.ClassLogic.Strategy;

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
