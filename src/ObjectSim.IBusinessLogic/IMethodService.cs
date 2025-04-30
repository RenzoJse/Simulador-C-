using ObjectSim.Domain;

namespace ObjectSim.IBusinessLogic;

public interface IMethodService
{
    Method Create(Method method);
    List<Method> GetAll();
    bool Delete(Guid id);
    Method GetById(Guid id);
    Method Update(Guid id, Method entity);
    Parameter AddParameter(Guid methodId, Parameter parameter);
    LocalVariable AddLocalVariable(Guid methodId, LocalVariable localVariable);
}
