using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using Attribute = ObjectSim.Domain.Attribute;

namespace ObjectSim.IBusinessLogic;

public interface IClassService
{
    public Class CreateClass(CreateClassArgs args);
    public Class GetById(Guid? classId);
    public void AddMethod(Guid? classId, Method method);
    public void AddAttribute(Guid? classId, Attribute attribute);
    public bool CanAddAttribute(Class classObj, Attribute attribute);
    public bool CanAddMethod(Class classObj, Method method);
    public void DeleteClass(Guid? classId);
    public void RemoveMethod(Guid? classId, Guid? methodId);
    public void RemoveAttribute(Guid? classId, Guid? attributeId);

}
