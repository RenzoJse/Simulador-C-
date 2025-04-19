using ObjectSim.BusinessLogic.ClassLogic.ClassBuilders;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;

namespace ObjectSim.BusinessLogic.ClassLogic;

public class ClassDirector(Builder classBuilder)
{
    public Class ConstructClass(CreateClassArgs args)
    {
        classBuilder.SetName(args.Name!);
        classBuilder.SetParent(args.Parent);
        classBuilder.SetAbstraction(args.IsAbstract);
        classBuilder.SetSealed(args.IsSealed);
        classBuilder.SetAttributes(args.Attributes);
        classBuilder.SetMethods(args.Methods);
        return null!;
    }
}
