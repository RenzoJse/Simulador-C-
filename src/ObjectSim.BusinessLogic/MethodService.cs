using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.BusinessLogic;
public class MethodService(IRepository<Method> methodRepository, IClassService classService) : IMethodService
{
    public Method CreateMethod(CreateMethodArgs methodsArgs)
    {
        if(methodsArgs is null)
        {
            throw new ArgumentNullException(nameof(methodsArgs), "Method arguments cannot be null.");
        }

        var method = new Method
        {
            Name = methodsArgs.Name,
            ClassId = methodsArgs.ClassId,
            Class = classService.GetById(methodsArgs.ClassId),
            Abstract = methodsArgs.IsAbstract ?? false,
            IsSealed = methodsArgs.IsSealed ?? false,
            IsOverride = methodsArgs.IsOverride ?? false,
            //Type = methodsArgs.Type, No se puede hacer aun.
            Parameters = methodsArgs.Parameters,
            LocalVariables = methodsArgs.LocalVariables,
            MethodsInvoke = methodsArgs.InvokeMethods
        };

        classService.AddMethod(methodsArgs.ClassId, method);

        methodRepository.Add(method);

        return method;
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

        return methods;
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
