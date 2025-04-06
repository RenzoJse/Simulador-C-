using ObjectSim.BusinessLogic.Args;

namespace ObjectSim.BusinessLogic;

public class ClassService
{
    public ClassService()
    {
    }

    public static void Create(CreateClassArgs args)
    {
        IsValidName(args.Name!);
    }

    private static void IsValidName(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        if(name.Length > 15)
        {
            throw new ArgumentException("Name cannot be longer than 15 characters");
        }

        if(name.Length < 3)
        {
            throw new ArgumentException("Name cannot be shorter than 3 characters");
        }
    }
}
