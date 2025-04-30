using ObjectSim.Domain;
using ObjectSim.Domain.Args;

namespace ObjectSim.IBusinessLogic;

public interface IMethodService
{
    Method CreateMethod(CreateMethodArgs method);
    List<Method> GetAll();
    bool Delete(Guid id);
    Method GetById(Guid id);
    Method Update(Guid id, Method entity);
    LocalVariable AddLocalVariable(Guid methodId, LocalVariable localVariable);
}
