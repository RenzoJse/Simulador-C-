using ObjectSim.ClassLogic.ClassBuilders;
using ObjectSim.ClassLogic.ClassBuilders.Builders;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.ClassLogic.Strategy;

public class ClassBuilderStrategy(
    IMethodService methodService,
    IAttributeService attributeService)
    : IBuilderStrategy
{
    public bool WhichIsMyBuilder(CreateClassArgs args)
    {
        return args is { IsInterface: false, IsAbstract: false };
    }

    public Builder CreateBuilder()
    {
        return new ClassBuilder(methodService, attributeService);
    }
}
