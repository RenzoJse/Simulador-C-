using ObjectSim.Domain;
using ObjectSim.IBusinessLogic;
using ObjectSim.IDataAccess;

namespace ObjectSim.BusinessLogic;
public class ParameterService(IParameterRepository<Parameter> parameterRepository) : IParameterService<Parameter>
{
    public Parameter Create(Parameter Entity)
    {
        throw new NotImplementedException();
    }
}
