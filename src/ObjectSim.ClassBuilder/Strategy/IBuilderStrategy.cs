using ObjectSim.ClassLogic.ClassBuilders;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.ClassLogic.Strategy;

public interface IBuilderStrategy
{
    bool WhichIsMyBuilder(CreateClassArgs args);
    public Builder CreateBuilder(IClassService classService);
}
