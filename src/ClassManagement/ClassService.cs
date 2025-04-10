using ClassManagement.ClassesBuilders;

namespace ClassManagement;

public class ClassService
{
    public static Class CreateClass(CreateClassArgs args, Builder builder)
    {
        var director = new ClassDirector(builder);
        return director.ConstructClass(args);
    }

}
