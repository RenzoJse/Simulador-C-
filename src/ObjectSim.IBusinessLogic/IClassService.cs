using ObjectSim.Domain;
using Attribute = ObjectSim.Domain.Attribute;

namespace ObjectSim.IBusinessLogic;

public interface IClassService
{
    public Class GetById(Guid? id);
    public void AddMethod(Guid id, Method method);
    public void AddAttribute(Guid id, Attribute attribute);
    public bool CanAddAttribute(Class classObj, Attribute attribute);
    public bool CanAddMethod(Class classObj, Method method);
}
