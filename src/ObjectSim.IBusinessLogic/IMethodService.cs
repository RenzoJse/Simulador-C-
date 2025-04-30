using ObjectSim.Domain;

namespace ObjectSim.IBusinessLogic;

public interface IMethodService
{
    Method Create(Method method);
    List<Method> GetAll();
    bool Delete(Guid id);
    Method GetById(Guid id);
    Method Update(Guid id, Method entity);
    LocalVariable AddLocalVariable(Guid methodId, LocalVariable localVariable);
}
