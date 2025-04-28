using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.BusinessLogic;
public class LocalVariableService(IRepository<LocalVariable> localVariableRepository) : ILocalVariableService
{
    public LocalVariable Create(LocalVariable entity)
    {
        if(entity == null)
        {
            throw new Exception("LocalVariable cannot be null");
        }

        var existLocalVariable = localVariableRepository.Exists(m => m.Name == entity.Name);
        if(existLocalVariable)
        {
            throw new Exception("LocalVariable already exist");
        }

        var localVariableToAdd = new LocalVariable
        {
            Id = entity.Id,
            Name = entity.Name,
            Type = entity.Type,
        };

        localVariableRepository.Add(localVariableToAdd);
        return localVariableToAdd;
    }
}
