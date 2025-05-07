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
    DataType AddParameter(Guid methodId, DataType parameter);
    DataType AddLocalVariable(Guid methodId, DataType localVariable);
    Method AddInvokeMethod(Guid methodId, List<CreateInvokeMethodArgs> invokeMethodArgs);

}
