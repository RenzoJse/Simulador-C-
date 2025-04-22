using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.BusinessLogic;
public class MethodService(IRepository<Method> methodRepository) : IMethodService
{
    public Method Create(Method Entity)
    {
        var existMethod = methodRepository.Exists(m => m.Name == Entity.Name);
        if(existMethod)
        {
            throw new Exception("Method already exist");
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

    public bool Delete(Guid id)
    {
        var method = methodRepository.Get(method1 => id == method1.Id);
        if(method == null)
        {
            throw new Exception("Method cannot be null.");
        }

        methodRepository.Delete(method);
        return true;
    }

    public List<Method> GetAll()
    {
        var methods = methodRepository.GetAll(method1 => method1.Id != Guid.Empty);
        if(methods == null || !methods.Any())
        {
            throw new Exception("No methods found.");
        }

        return (List<Method>)methods;
    }

    public Method GetById(Guid id)
    {
        try
        {
            return methodRepository.Get(method1 => id == method1.Id)!;
        }
        catch(Exception)
        {
            throw new InvalidOperationException("Not found method.");
        }
    }

    public Method Update(Guid id, Method entity)
    {
        Method method = methodRepository.Get(method1 => id == method1.Id)!;
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
