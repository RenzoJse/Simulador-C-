using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.BusinessLogic;
public class MethodService(IRepository<Method> methodRepository, IRepository<Class> classRepository, IDataTypeService dataTypeService,
    IInvokeMethodService invokeMethodService) : IMethodService, IMethodServiceCreate
{
    #region CreateMethod

    public Method CreateMethod(CreateMethodArgs methodArgs)
    {
        ValidateMethodArgsNotNull(methodArgs);
        ValidateTypeIdExists(methodArgs.TypeId);
        var method = BuildMethodFromArgs(methodArgs);

        AddMethodToClass(methodArgs.ClassId, method);

        SaveMethod(method);

        return method;
    }

    private Guid ValidateTypeIdExists(Guid typeId)
    {
        if(typeId == Guid.Empty)
        {
            throw new ArgumentException("Type ID cannot be empty.");
        }

        return dataTypeService.GetById(typeId).Id;
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

        return new Method
        {
            Name = methodArgs.Name,
            ClassId = methodArgs.ClassId,
            Abstract = methodArgs.IsAbstract ?? false,
            IsSealed = methodArgs.IsSealed ?? false,
            IsOverride = methodArgs.IsOverride ?? false,
            IsVirtual = methodArgs.IsVirtual ?? false,
            TypeId = methodArgs.TypeId,
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

        classObj.CanAddMethod(method);

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
        var method = GetMethodById(id);
        methodRepository.Delete(method);
        return true;
    }

    private Method GetMethodById(Guid id)
    {
        return methodRepository.Get(m => m.Id == id)
               ?? throw new KeyNotFoundException($"Method with ID {id} not found.");
    }

    #endregion

    #region GetAll

    public List<Method> GetAll()
    {
        var methods = methodRepository.GetAll(m => m.Id != Guid.Empty)?.ToList();
        if(methods == null || methods.Count == 0)
        {
            throw new Exception("No methods found.");
        }

        return methods;
    }

    #endregion

    #region GetById

    public Method GetById(Guid id)
    {
        return methodRepository.Get(m => m.Id == id)
               ?? throw new KeyNotFoundException($"Method with ID {id} not found.");
    }

    #endregion

    #region Update

    public Method Update(Guid id, Method updatedMethod)
    {
        var method = GetMethodById(id);

        ValidateMethodName(updatedMethod.Name!);

        UpdateMethodProperties(method, updatedMethod);

        methodRepository.Update(method);

        return method;
    }

    private static void ValidateMethodName(string name)
    {
        if(string.IsNullOrWhiteSpace(name))
        {
            throw new Exception("Incorrect name method");
        }
    }

    private static void UpdateMethodProperties(Method method, Method updated)
    {
        method.Name = updated.Name;
        method.TypeId = updated.TypeId;
        method.Abstract = updated.Abstract;
        method.IsSealed = updated.IsSealed;
        method.Parameters = updated.Parameters;
        method.LocalVariables = updated.LocalVariables;
        method.Accessibility = updated.Accessibility;
    }

    #endregion

    #region AddParameter

    public DataType AddParameter(Guid methodId, DataType parameter)
    {
        var method = GetMethodById(methodId);

        if(method.Parameters.Any(p => p.Name == parameter.Name))
        {
            throw new Exception("Parameter already exists in this method");
        }

        method.Parameters.Add(parameter);
        methodRepository.Update(method);

        return parameter;
    }

    #endregion

    #region AddLocalVariable

    public DataType AddLocalVariable(Guid methodId, DataType localVariable)
    {
        var method = GetMethodById(methodId);

        if(method.LocalVariables.Any(lv => lv.Name == localVariable.Name))
        {
            throw new Exception("LocalVariable already exists in this method");
        }

        method.LocalVariables.Add(localVariable);
        methodRepository.Update(method);

        return localVariable;
    }

    #endregion

    #region AddInvokeMethod

    public Method AddInvokeMethod(Guid methodId, List<CreateInvokeMethodArgs> invokeMethodArgs)
    {
        ValidateInvokeMethodArgs(invokeMethodArgs);

        var method = GetMethodById(methodId);
        AddInvokeMethods(invokeMethodArgs, method);

        methodRepository.Update(method);

        return method;
    }

    private static void ValidateInvokeMethodArgs(List<CreateInvokeMethodArgs> invokeMethodArgs)
    {
        if(invokeMethodArgs == null || invokeMethodArgs.Count == 0)
        {
            throw new ArgumentException("Invoke method arguments cannot be null or empty.");
        }
    }

    private void AddInvokeMethods(List<CreateInvokeMethodArgs> invokeMethodArgs, Method method)
    {
        foreach (var invokeArg in invokeMethodArgs)
        {
            var methodToInvoke = GetMethodToInvoke(invokeArg.InvokeMethodId);
            ValidateCanAddInvokeMethod(method, methodToInvoke, invokeArg.Reference);
            ValidateInvokeMethodReachable(method, invokeArg.InvokeMethodId);
            CreateInvokeMethod(invokeArg, method);
        }
    }

    private Method GetMethodToInvoke(Guid invokeMethodId)
    {
        return methodRepository.Get(m => m.Id == invokeMethodId)
               ?? throw new Exception($"Method to invoke with id {invokeMethodId} not found");
    }

    private void ValidateCanAddInvokeMethod(Method method, Method methodToInvoke, string reference)
    {
        method.CanAddInvokeMethod(methodToInvoke, GetClassById(method.ClassId), reference);
    }

    private void ValidateInvokeMethodReachable(Method method, Guid invokeMethodId)
    {
        if (method.Parameters.Any(param => param.Id != invokeMethodId) ||
            method.LocalVariables.Any(localVar => localVar.Id != invokeMethodId))
        {
            throw new ArgumentException("The invoked method must be reachable from the current method.");
        }
    }

    private void CreateInvokeMethod(CreateInvokeMethodArgs invokeArg, Method method)
    {
        invokeMethodService.CreateInvokeMethod(invokeArg, method);
    }g

    #endregion

}
