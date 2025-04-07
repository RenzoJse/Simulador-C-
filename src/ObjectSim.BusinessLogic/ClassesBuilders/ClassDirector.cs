using ObjectSim.Domain.Args;

namespace ObjectSim.BusinessLogic.ClassesBuilders;

public class ClassDirector
{
    private readonly Builder _builder;

    public ClassDirector(Builder classBuilder)
    {
        _builder = classBuilder;
    }

    public void ConstructClass(CreateClassArgs args)
    {
        _builder.SetName(args.Name!);
        _builder.SetAbstraction(args.IsAbstract);
    }
}
