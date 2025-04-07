using ObjectSim.BusinessLogic.ClassesBuilders;
using ObjectSim.Domain.Args;

namespace ObjectSim.BusinessLogic;

public class ClassService
{
    public static void Create(CreateClassArgs args, Builder builder)
    {
        var director = new ClassDirector(builder);
        director.ConstructClass(args);
    }

}
