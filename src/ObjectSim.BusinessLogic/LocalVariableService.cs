using ObjectSim.Domain;
using ObjectSim.IBusinessLogic;
using ObjectSim.IDataAccess;

namespace ObjectSim.BusinessLogic;
public class LocalVariableService(ILocalVariableRepository<LocalVariable> localVariableRepostiroy) : ILocalVariableService<LocalVariable>
{
    public LocalVariable Create(LocalVariable Entity)
    {
        throw new NotImplementedException();
    }
}
