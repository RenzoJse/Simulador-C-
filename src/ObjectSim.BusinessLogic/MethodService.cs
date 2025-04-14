using ObjectSim.Domain;
using ObjectSim.IBusinessLogic;
using ObjectSim.IDataAccess;

namespace ObjectSim.BusinessLogic;
public class MethodService(IMethodRepository<Method> methodRepository) : IMethodService<Method>
{
    public Method Create(Method Entity)
    {
        if (Entity == null) 
        {
            throw new Exception("Method cannot be null");
        }

        var existMethod = methodRepository.Exist(m => m.Name == Entity.Name);
        if(existMethod)
        {
            throw new Exception("Method already exist");
        }

        if(Entity.Name == string.Empty)
        {
            throw new Exception("Method name cannot be empty");
        }

        var methodToAdd = new Method
        {
            Name = Entity.Name,
            Type = Entity.Type,
            Abstract = Entity.Abstract,
            IsSealed = Entity.IsSealed,
            Accessibility = Entity.Accessibility,
            Parameters = Entity.Parameters,
            LocalVariables = Entity.LocalVariables,
        };

        methodRepository.Add(methodToAdd);
        return methodToAdd;
    }

    public bool Delete(int id)
    {
        throw new NotImplementedException();
    }

    public List<Method> GetAll()
    {
        var methods = methodRepository.GetAll();
        if(methods == null || !methods.Any())
        {
            throw new Exception("No methods found.");
        }

        return (List<Method>)methods;
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
