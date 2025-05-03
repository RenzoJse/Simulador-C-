using ObjectSim.ClassConstructor.ClassBuilders;
using ObjectSim.ClassConstructor.ClassBuilders.Builders;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.ClassConstructor.Strategy;

public class ClassBuilderStrategy(
    IMethodServiceCreate methodService,
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
