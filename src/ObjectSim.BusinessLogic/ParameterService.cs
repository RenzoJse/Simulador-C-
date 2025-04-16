using ObjectSim.Domain;
using ObjectSim.IBusinessLogic;
using ObjectSim.IDataAccess;

namespace ObjectSim.BusinessLogic;
public class ParameterService(IParameterRepository<Parameter> parameterRepository) : IParameterService<Parameter>
{
    public Parameter Create(Parameter Entity)
    {
        if(Entity == null)
        {
            throw new Exception("Parameter cannot be null");
        }

        var existParameter = parameterRepository.Exist(m => m.Name == Entity.Name);
        if(existParameter)
        {
            throw new Exception("Parameter already exist");
        }

        var parameterToAdd = new Parameter
        {
            Id = Entity.Id,
            Name = Entity.Name,
            Type = Entity.Type,
        };

        parameterRepository.Add(parameterToAdd);
        return parameterToAdd;
    }
}
