using ObjectSim.ClassLogic.ClassBuilders;
using ObjectSim.ClassLogic.ClassBuilders.Builders;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.ClassLogic.Strategy;

public class InterfaceBuilderStrategy(IAttributeService attributeService) : IBuilderStrategy
{
    public bool WhichIsMyBuilder(CreateClassArgs args)
    {
        return args.IsInterface == true;
    }

    public Builder CreateBuilder(IClassService classService)
    {
        return new InterfaceBuilder(classService, attributeService);
    }
}
