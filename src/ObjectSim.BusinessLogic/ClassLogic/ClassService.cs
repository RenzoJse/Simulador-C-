using ObjectSim.BusinessLogic.ClassLogic.ClassBuilders;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;

namespace ObjectSim.BusinessLogic.ClassLogic;

public class ClassService
{
    public static Class CreateClass(CreateClassArgs args, Builder builder)
    {
        var director = new ClassDirector(builder);
        return director.ConstructClass(args);
    }

}
