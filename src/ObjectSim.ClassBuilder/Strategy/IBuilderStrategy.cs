using ObjectSim.ClassConstructor.ClassBuilders;
using ObjectSim.Domain.Args;

namespace ObjectSim.ClassConstructor.Strategy;

public interface IBuilderStrategy
{
    bool WhichIsMyBuilder(CreateClassArgs args);
    public Builder CreateBuilder();
}
