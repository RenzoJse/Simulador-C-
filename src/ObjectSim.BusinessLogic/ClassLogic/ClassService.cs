using ObjectSim.BusinessLogic.ClassLogic.ClassBuilders;
using ObjectSim.BusinessLogic.ClassLogic.Strategy;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;
using Attribute = ObjectSim.Domain.Attribute;

namespace ObjectSim.BusinessLogic.ClassLogic;

public class ClassService(List<IBuilderStrategy> strategies) : IClassService
{

    public Class CreateClass(CreateClassArgs args)
    {
       var builder = GetBuilder(args);
        return null!;
    }

    private Builder GetBuilder(CreateClassArgs args)
    {
        var strategy = strategies.FirstOrDefault(x => x.WhichIsMyBuilder(args));

        return strategy!.CreateBuilder();
    }

    public Class GetById(Guid? classId)
    {
        throw new NotImplementedException();
    }

    public void AddMethod(Guid classId, Method method)
    {
        throw new NotImplementedException();
    }

    public void AddAttribute(Guid classId, Attribute attribute)
    {
        throw new NotImplementedException();
    }

    public bool CanAddAttribute(Class classObj, Attribute attribute)
    {
        throw new NotImplementedException();
    }

    public bool CanAddMethod(Class classObj, Method method)
    {
        throw new NotImplementedException();
    }
}
