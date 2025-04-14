using ObjectSim.Domain;
using ObjectSim.IBusinessLogic;
using ObjectSim.IDataAccess;

namespace ObjectSim.BusinessLogic;
public class MethodService(IMethodRepository<Method> methodRepository) : IMethodService<Method>
{
    public Method Create(Method Entity)
    {
        if(Entity == null)
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
        var method = methodRepository.GetById(id);
        if(method == null)
        {
            throw new Exception("Method cannot be null.");
        }

        methodRepository.Remove(method);
        return true;
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
        try
        {
            return methodRepository.GetById(id);
        }
        catch(Exception)
        {
            throw new InvalidOperationException("Not found method.");
        }
    }

    public Method Update(int id, Method entity)
    {
        Method method = methodRepository.GetById(id);
        if(entity.Name == string.Empty)
        {
            throw new Exception("Incorrect name method");
        }
        method.Name = entity.Name;
        method.Type = entity.Type;
        method.Abstract = entity.Abstract;
        method.IsSealed = entity.IsSealed;
        method.Parameters = entity.Parameters;
        method.LocalVariables = entity.LocalVariables;
        method.Accessibility = entity.Accessibility;

        methodRepository.Update(method);
        return method;
    }
}
