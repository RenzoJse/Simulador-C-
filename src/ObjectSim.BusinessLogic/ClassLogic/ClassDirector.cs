using ObjectSim.BusinessLogic.ClassLogic.ClassBuilders;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;

namespace ObjectSim.BusinessLogic.ClassLogic;

public class ClassDirector
{
    private readonly Builder _builder;

    public ClassDirector(Builder classBuilder)
    {
        _builder = classBuilder;
    }

    public Class ConstructClass(CreateClassArgs args)
    {
        _builder.SetName(args.Name!);
        _builder.SetAbstraction(args.IsAbstract);
        return null!;
    }
}
