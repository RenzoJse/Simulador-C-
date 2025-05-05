using ObjectSim.ClassConstructor.ClassBuilders;
using ObjectSim.ClassConstructor.ClassBuilders.Builders;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.ClassConstructor.Strategy;

public class AbstractBuilderStrategy(IMethodServiceCreate methodService, IAttributeService attributeService) : IBuilderStrategy
{
    public bool WhichIsMyBuilder(CreateClassArgs args)
    {
        return args is { IsInterface: false, IsAbstract: true };
    }

    public Builder CreateBuilder()
    {
        return new AbstractBuilder(methodService, attributeService);
    }
}
