using ObjectSim.BusinessLogic.Args;

namespace ObjectSim.BusinessLogic;

public class ClassService
{
    public ClassService()
    {
    }

    public void Create(CreateClassArgs args)
    {
        if(args.Name == null)
        {
            throw new ArgumentNullException(nameof(args.Name));
        }
    }
}
