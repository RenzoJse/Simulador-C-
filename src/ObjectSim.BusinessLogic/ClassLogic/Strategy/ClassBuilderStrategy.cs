using ObjectSim.BusinessLogic.ClassLogic.ClassBuilders;
using ObjectSim.BusinessLogic.ClassLogic.ClassBuilders.Builders;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.BusinessLogic.ClassLogic.Strategy;

public class ClassBuilderStrategy(
    IMethodService methodService)
    : IBuilderStrategy
{
    public bool WhichIsMyBuilder(CreateClassArgs args)
    {
        return args is { IsInterface: false, IsAbstract: false };
    }

    public Builder CreateBuilder(IClassService classService, IAttributeService attributeService)
    {
        return new ClassBuilder(methodService, classService, attributeService);
    }
}
