using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;
using ValueType = ObjectSim.Domain.ValueType;

namespace ObjectSim.BusinessLogic;
public class MethodService(IRepository<Method> methodRepository, IRepository<Class> classRepository, IDataTypeService dataTypeService) : IMethodService, IMethodServiceCreate
{
    public Method CreateMethod(CreateMethodArgs methodsArgs)
    {
        ValidateNullMethodArgs(methodsArgs);

        var method = BuildMethod(methodsArgs);

        AddMethod(methodsArgs.ClassId, method);

        methodRepository.Add(method);

        return method;
    }

    private static void ValidateNullMethodArgs(CreateMethodArgs methodsArgs)
    {
        if(methodsArgs is null)
        {
            throw new ArgumentNullException(nameof(methodsArgs), "Method arguments cannot be null.");
        }
    }

    private Method BuildMethod(CreateMethodArgs methodsArgs)
    {
        List<DataType> parameters = [];
        parameters.AddRange(methodsArgs.Parameters.Select(dataTypeService.CreateDataType));
        List<DataType> localVariables = [];
        localVariables.AddRange(methodsArgs.LocalVariables.Select(dataTypeService.CreateDataType));

        var method = new Method
        {
            Name = methodsArgs.Name,
            ClassId = methodsArgs.ClassId,
            Abstract = methodsArgs.IsAbstract ?? false,
            IsSealed = methodsArgs.IsSealed ?? false,
            IsOverride = methodsArgs.IsOverride ?? false,
            Type = dataTypeService.CreateDataType(methodsArgs.Type),
            TypeId = dataTypeService.CreateDataType(methodsArgs.Type).Id,
            Parameters = parameters,
            LocalVariables = localVariables,
            MethodsInvoke = GetInvokeMethods(methodsArgs.InvokeMethods)
        };

        return method;
    }

    private Class GetClassById(Guid classId)
    {
        return classRepository.Get(c => c.Id == classId) ?? throw new ArgumentException("Class not found.");
    }

    private void AddMethod(Guid classId, Method? method)
    {
        ArgumentNullException.ThrowIfNull(classId);
        ArgumentNullException.ThrowIfNull(method);

        var classObj = GetClassById(classId);

        classObj.CanAddMethod(classObj, method); //Tengo que ver lo de si abstract haga la clase abstract

        classObj.Methods!.Add(method);
    }

    private List<Method> GetInvokeMethods(List<Guid> invokeMethodIds)
    {
        if(invokeMethodIds.Count == 0)
        {
            return [];
        }

        var invokeMethods = new List<Method>();

        foreach(var methodId in invokeMethodIds)
        {
            var invokeMethod = methodRepository.Get(m => m.Id == methodId);
            if(invokeMethod == null)
            {
                throw new Exception($"Method with ID {methodId} not found.");
            }

            invokeMethods.Add(invokeMethod);
        }

        return invokeMethods;
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

    public DataType AddParameter(Guid methodId, DataType parameter)
    {
        var method = methodRepository.Get(m => m.Id == methodId)
            ?? throw new Exception("Method not found");

        if(method.Parameters.Any(p => p.Name == parameter.Name))
        {
            throw new Exception("Parameter already exists in this method");
        }

        method.Parameters.Add(parameter);
        methodRepository.Update(method);

        return parameter;
    }

    public DataType AddLocalVariable(Guid methodId, DataType localVariable)
    {
        var method = methodRepository.Get(m => m.Id == methodId)
            ?? throw new Exception("Method not found");

        if(method.LocalVariables.Any(lv => lv.Name == localVariable.Name))
        {
            throw new Exception("LocalVariable already exists in this method");
        }

        method.LocalVariables.Add(localVariable);
        methodRepository.Update(method);

        return localVariable;
    }
}
