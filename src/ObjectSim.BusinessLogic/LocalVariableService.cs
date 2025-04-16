using ObjectSim.Domain;
using ObjectSim.IBusinessLogic;
using ObjectSim.IDataAccess;

namespace ObjectSim.BusinessLogic;
public class LocalVariableService(ILocalVariableRepository<LocalVariable> localVariableRepostiroy) : ILocalVariableService<LocalVariable>
{
    public LocalVariable Create(LocalVariable Entity)
    {
        if(Entity == null)
        {
            throw new Exception("LocalVariable cannot be null");
        }

        var existLocalVariable = localVariableRepostiroy.Exist(m => m.Name == Entity.Name);
        if(existLocalVariable)
        {
            throw new Exception("LocalVariable already exist");
        }

        var localVariableToAdd = new LocalVariable
        {
            Id = Entity.Id,
            Name = Entity.Name,
            Type = Entity.Type,
        };

        localVariableRepostiroy.Add(localVariableToAdd);
        return localVariableToAdd;
    }
}
