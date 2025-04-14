using ObjectSim.Domain;
using ObjectSim.IBusinessLogic;
using ObjectSim.IDataAccess;

namespace ObjectSim.BusinessLogic;
public class MethodService(IMethodRepository<Method> methodRepository) : IMethodService<Method>
{
    public Method Create(Method Entity)
    {
        methodRepository.GetAll();
        throw new NotImplementedException();
    }

    public bool Delete(int id)
    {
        throw new NotImplementedException();
    }

    public List<Method> GetAll()
    {
        throw new NotImplementedException();
    }

    public Method GetById(int id)
    {
        throw new NotImplementedException();
    }

    public Method Update(int id, Method entity)
    {
        throw new NotImplementedException();
    }
}
