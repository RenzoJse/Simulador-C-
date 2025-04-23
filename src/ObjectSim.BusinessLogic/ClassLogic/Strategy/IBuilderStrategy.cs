using ObjectSim.BusinessLogic.ClassLogic.ClassBuilders;
using ObjectSim.Domain.Args;

namespace ObjectSim.BusinessLogic.ClassLogic.Strategy;

interface IBuilderStrategy
{
    bool WhichIsMyBuilder(CreateClassArgs args);
    public Builder CreateBuilder();
}
