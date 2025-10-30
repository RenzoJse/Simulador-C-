using ObjectSim.Domain;
using ObjectSim.Domain.Args;

namespace ObjectSim.IBusinessLogic;

public interface IClassService
{
    public Class CreateClass(CreateClassArgs args);
    public Class GetById(Guid? classId);
    List<Class> GetAll();
    public void DeleteClass(Guid? classId);
    public void RemoveMethod(Guid? classId, Guid? methodId);
    public void RemoveAttribute(Guid classId, Guid attributeId);
    public void UpdateClass(Guid classId, string newName);
    public Guid GetIdByName(string name);
}
