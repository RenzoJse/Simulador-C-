using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;
using Attribute = ObjectSim.Domain.Attribute;

namespace ObjectSim.BusinessLogic.ClassLogic;

public class ClassService : IClassService
{
    public Class CreateClass(CreateClassArgs args)
    {
        throw new NotImplementedException();
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
