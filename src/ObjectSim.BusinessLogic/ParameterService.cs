using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.BusinessLogic;
public class ParameterService(IRepository<Parameter> parameterRepository) : IParameterService
{
    public Parameter Create(Parameter entity)
    {
        if(entity == null)
        {
            throw new Exception("Parameter cannot be null");
        }

        var existParameter = parameterRepository.Exists(m => m.Name == entity.Name);
        if(existParameter)
        {
            throw new Exception("Parameter already exist");
        }

        var parameterToAdd = new Parameter
        {
            Id = entity.Id,
            Name = entity.Name,
            Type = entity.Type,
        };

        parameterRepository.Add(parameterToAdd);
        return parameterToAdd;
    }
}
