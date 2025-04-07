using ObjectSim.BusinessLogic.ClassesBuilders;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;

namespace ObjectSim.BusinessLogic;

public class ClassService
{
    public static Class CreateClass(CreateClassArgs args, Builder builder)
    {
        var director = new ClassDirector(builder);
        return director.ConstructClass(args);
    }

}
