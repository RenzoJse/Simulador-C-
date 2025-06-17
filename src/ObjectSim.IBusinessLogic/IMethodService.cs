using ObjectSim.Domain;
using ObjectSim.Domain.Args;

namespace ObjectSim.IBusinessLogic;

public interface IMethodService
{
    Method CreateMethod(CreateMethodArgs methodArgs);
    List<Method> GetAll();
    bool Delete(Guid id);
    Method GetById(Guid id);
    Method Update(Guid id, Method entity);
    Variable AddParameter(Guid methodId, Variable parameter);
    Variable AddLocalVariable(Guid methodId, Variable localVariable);
    Method AddInvokeMethod(Guid methodId, List<CreateInvokeMethodArgs> invokeMethodArgs);
    Method GetIdByName(string name);

}
