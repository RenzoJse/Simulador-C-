using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.BusinessLogic;
public class MethodService(IRepository<Method> methodRepository, IRepository<Class> classRepository, IDataTypeService dataTypeService) : IMethodService, IMethodServiceCreate
{

    #region CreateMethod

    public Method CreateMethod(CreateMethodArgs methodArgs)
    {
        ValidateMethodArgsNotNull(methodArgs);

        var method = BuildMethodFromArgs(methodArgs);

        AddMethodToClass(methodArgs.ClassId, method);

        SaveMethod(method);

        return method;
    }

    private static void ValidateMethodArgsNotNull(CreateMethodArgs methodArgs)
    {
        if(methodArgs is null)
        {
            throw new ArgumentNullException(nameof(methodArgs), "Method arguments cannot be null.");
        }
    }

    private Method BuildMethodFromArgs(CreateMethodArgs methodArgs)
    {
        var parameters = BuildDataTypes(methodArgs.Parameters);
        var localVariables = BuildDataTypes(methodArgs.LocalVariables);

        var type = dataTypeService.CreateDataType(methodArgs.Type);

        return new Method
        {
            Name = methodArgs.Name,
            ClassId = methodArgs.ClassId,
            Abstract = methodArgs.IsAbstract ?? false,
            IsSealed = methodArgs.IsSealed ?? false,
            IsOverride = methodArgs.IsOverride ?? false,
            Type = type,
            TypeId = type.Id,
            Parameters = parameters,
            LocalVariables = localVariables,
            MethodsInvoke = []
        };
    }

    private List<DataType> BuildDataTypes(IEnumerable<CreateDataTypeArgs> dataTypeArgs)
    {
        return dataTypeArgs.Select(dataTypeService.CreateDataType).ToList();
    }

    private void AddMethodToClass(Guid classId, Method method)
    {
        ArgumentNullException.ThrowIfNull(classId);
        ArgumentNullException.ThrowIfNull(method);

        var classObj = GetClassById(classId);

        classObj.CanAddMethod(method); //TODO Tengo que ver lo de si abstract haga la clase abstract

        classObj.Methods!.Add(method);
    }

    private void SaveMethod(Method method)
    {
        methodRepository.Add(method);
    }

    private Class GetClassById(Guid classId)
    {
        return classRepository.Get(c => c.Id == classId)
               ?? throw new ArgumentException("Class not found.");
    }

    #endregion

    #region Delete

    public bool Delete(Guid id)
    {
        var method = methodRepository.Get(method1 => id == method1.Id);
        if(method == null)
        {
            throw new KeyNotFoundException($"Method with id {id} not found.");
        }

        methodRepository.Delete(method);
        return true;
    }

    #endregion

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
        var method = methodRepository.Get(method1 => id == method1.Id);
        if (method == null)
        {
            throw new KeyNotFoundException($"Method with ID {id} not found.");
        }

        return method;
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

    #region AddInvokeMethod

    public Method AddInvokeMethod(Guid methodId, List<CreateInvokeMethodArgs> invokeMethodArgs)
    {
        if(invokeMethodArgs == null || invokeMethodArgs.Count == 0)
        {
            throw new ArgumentException("Invoke method arguments cannot be null or empty.");
        }

        var method = GetById(methodId);
        var result = CreateInvokeMethods(invokeMethodArgs, method);
        method.MethodsInvoke.AddRange(result);
        methodRepository.Update(method);
        return method;
    }

    private List<InvokeMethod> CreateInvokeMethods(List<CreateInvokeMethodArgs> invokeMethodArgs, Method method)
    {
        List<InvokeMethod> invokeMethods = [];
        foreach (var invokeMethod in invokeMethodArgs)
        {
            var methodToInvoke = methodRepository.Get(m => m.Id == invokeMethod.InvokeMethodId);
            if (methodToInvoke == null)
            {
                throw new Exception($"Method to invoke with id {invokeMethod.InvokeMethodId} not found");
            }
            method.CanAddInvokeMethod(methodToInvoke, GetClassById(method.ClassId), invokeMethod.Reference);
            var newInvokeMethod = new InvokeMethod(method.Id, invokeMethod.InvokeMethodId, invokeMethod.Reference);
            invokeMethods.Add(newInvokeMethod);
        }

        return invokeMethods;
    }

    #endregion
}
