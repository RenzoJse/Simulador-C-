using ObjectSim.BusinessLogic.ClassLogic.ClassBuilders;
using ObjectSim.BusinessLogic.ClassLogic.ClassBuilders.Builders;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.BusinessLogic.ClassLogic.Strategy;

public class ClassBuilderStrategy(
    IMethodService methodService,
    IAttributeService attributeService,
    IClassService classService)
    : IBuilderStrategy
{
    public bool WhichIsMyBuilder(CreateClassArgs args)
    {
        return args is { IsInterface: false, IsAbstract: false };
    }

    public Builder CreateBuilder()
    {
        return new ClassBuilder(methodService, classService, attributeService);
    }
}
