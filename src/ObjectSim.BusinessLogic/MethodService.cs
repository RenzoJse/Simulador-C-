using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;
using Attribute = ObjectSim.Domain.Attribute;

namespace ObjectSim.BusinessLogic;
public class MethodService(IRepository<Variable> variableRepository, IRepository<Method> methodRepository, IRepository<Class> classRepository, IDataTypeService dataTypeService,
    IInvokeMethodService invokeMethodService) : IMethodService, IMethodServiceCreate
{

    #region BuilderCreateMethod

    public Method BuilderCreateMethod(CreateMethodArgs methodArgs)
    {
        ValidateMethodArgsNotNull(methodArgs);
        ValidateTypeIdExists(methodArgs.TypeId);
        var method = BuildMethodFromArgs(methodArgs);

        SetMethodVariablesBuilder(method, methodArgs);

        return method;
    }

    private static void SetMethodVariablesBuilder(Method method, CreateMethodArgs methodArgs)
    {
        method.LocalVariables = BuildVariablesBuilder(methodArgs.LocalVariables, method);
        method.Parameters = BuildVariablesBuilder(methodArgs.Parameters, method);
    }

    private static List<Variable> BuildVariablesBuilder(IEnumerable<CreateVariableArgs> variablesArgs, Method method)
    {
        return variablesArgs.Select(variableArgs => new Variable(variableArgs.ClassId, variableArgs.Name, method)).ToList();
    }

    #endregion

    #region CreateMethod

    public Method CreateMethod(CreateMethodArgs methodArgs)
    {
        ValidateMethodArgsNotNull(methodArgs);
        ValidateTypeIdExists(methodArgs.TypeId);
        var method = BuildMethodFromArgs(methodArgs);

        AddMethodToClass(methodArgs.ClassId, method);
        SaveMethod(method);

        SetMethodVariables(method, methodArgs);
        if(methodArgs.InvokeMethods.Count > 0)
        {
            method = SetMethodInvokes(method, methodArgs);
        }

        return method;
    }

    private void SetMethodVariables(Method method, CreateMethodArgs methodArgs)
    {
        method.LocalVariables = BuildVariables(methodArgs.LocalVariables, method);
        method.Parameters = BuildVariables(methodArgs.Parameters, method);
    }

    private Method SetMethodInvokes(Method method, CreateMethodArgs methodArgs)
    {
        return AddInvokeMethod(method.Id, methodArgs.InvokeMethods);
    }

    private Guid ValidateTypeIdExists(Guid typeId)
    {
        if(typeId == Guid.Empty)
        {
            throw new ArgumentException("Name ID cannot be empty.");
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

    private static Method BuildMethodFromArgs(CreateMethodArgs methodArgs)
    {
        var result = new Method
        {
            Name = methodArgs.Name,
            ClassId = methodArgs.ClassId,
            Abstract = methodArgs.IsAbstract ?? false,
            IsSealed = methodArgs.IsSealed ?? false,
            IsOverride = methodArgs.IsOverride ?? false,
            IsVirtual = methodArgs.IsVirtual ?? false,
            IsStatic = methodArgs.IsStatic ?? false,
            TypeId = methodArgs.TypeId,
            Parameters = [],
            LocalVariables = [],
            MethodsInvoke = []
        };

        return result;
    }

    private List<Variable> BuildVariables(IEnumerable<CreateVariableArgs> variablesArgs, Method method)
    {
        var variables = new List<Variable>();
        foreach(var variableArgs in variablesArgs)
        {
            var newVariable = new Variable(variableArgs.ClassId, variableArgs.Name, method);
            variableRepository.Add(newVariable);
            variables.Add(newVariable);
        }

        return variables;
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
        var methods = methodRepository.GetAll(m => m.Id != Guid.Empty).ToList();
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

    public Variable AddParameter(Guid methodId, Variable parameter)
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

    public Variable AddLocalVariable(Guid methodId, Variable localVariable)
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
        foreach(var invokeArg in invokeMethodArgs)
        {
            var methodToInvoke = GetMethodToInvoke(invokeArg.InvokeMethodId);
            ValidateCanAddInvokeMethod(method, methodToInvoke, invokeArg.Reference);
            ValidateInvokeMethodReachable(method, invokeArg.InvokeMethodId, invokeArg.Reference);
            _ = CreateInvokeMethod(invokeArg, method);
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

    private void ValidateInvokeMethodReachable(Method method, Guid invokeMethodId, string reference)
    {
        var invokeMethod = methodRepository.Get(m => m.Id == invokeMethodId)
                              ?? throw new ArgumentException($"Method to invoke with id {invokeMethodId} not found");
        var currentClass = classRepository.Get(c => c.Id == method.ClassId)
                             ?? throw new ArgumentException($"Class with id {method.ClassId} not found");

        if(IsReferenceInMethodParameters(method, reference))
        {
            return;
        }

        if(IsReferenceInClassAttributes(currentClass, reference))
        {
            ValidateInvokeThroughClassAttribute(currentClass, invokeMethod, invokeMethodId, reference);
        }

        if(!IsInSameClass(method, invokeMethod))
        {
            ValidateInvokeFromOtherClass(method, invokeMethod, reference);
        }

        if(!IsReachable(method, currentClass, invokeMethod, invokeMethodId, reference))
        {
            throw new ArgumentException(
                $"Invoke method with id {invokeMethodId} is not reachable from method {method.Name}");
        }
    }

    private static bool IsReferenceInMethodParameters(Method method, string reference)
    {
        return method.Parameters?.Any(p => p.Name == reference) ?? false;
    }

    private static bool IsReferenceInClassAttributes(Class currentClass, string reference)
    {
        return currentClass.Attributes?.Any(a => a.Name == reference) ?? false;
    }

    private void ValidateInvokeThroughClassAttribute(Class currentClass, Method invokeMethod, Guid invokeMethodId,
        string reference)
    {
        foreach(Attribute attribute in currentClass.Attributes!)
        {
            var attributeClass = classRepository.Get(c => c.Id == attribute.ClassId)
                                   ?? throw new ArgumentException(
                                       $"Attribute class with id {attribute.ClassId} not found");

            if(invokeMethod.IsStatic &&
               !string.Equals(reference, attributeClass.Name, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new ArgumentException(
                    $"Cant invoke static attribute {attribute.Name} from class {attributeClass.Name} using reference {reference}");
            }

            ValidateMethodInClassOrParents(invokeMethodId, attributeClass);
        }
    }

    private static bool IsInSameClass(Method method, Method invokeMethod)
    {
        return invokeMethod.ClassId == method.ClassId;
    }

    private void ValidateInvokeFromOtherClass(Method method, Method invokeMethod, string reference)
    {
        Class classOfInvokeMethod = classRepository.Get(c => c.Id == invokeMethod.ClassId)
                                    ?? throw new ArgumentException($"Class with id {invokeMethod.ClassId} not found");

        if(invokeMethod.IsStatic &&
           !string.Equals(reference, classOfInvokeMethod.Name, StringComparison.CurrentCultureIgnoreCase))
        {
            throw new ArgumentException(
                $"Cant invoke static method {invokeMethod.Name} from class {classOfInvokeMethod.Name} using reference {reference}");
        }

        if(invokeMethod.Accessibility != Method.MethodAccessibility.Public)
        {
            throw new ArgumentException("Cannot invoke a non-public method from another class.");
        }
    }

    private static bool IsReachable(Method method, Class currentClass, Method invokeMethod, Guid invokeMethodId,
        string reference)
    {
        return IsInSameClass(method, invokeMethod)
               || IsMethodInClass(currentClass, invokeMethodId)
               || IsReferenceInClassAttributes(currentClass, reference)
               || IsReferenceInLocalVariables(method, invokeMethod.Name!);
    }

    private static bool IsMethodInClass(Class currentClass, Guid invokeMethodId)
    {
        return currentClass.Methods?.Any(m => m.Id == invokeMethodId) ?? false;
    }

    private static bool IsReferenceInLocalVariables(Method method, string name)
    {
        return method.LocalVariables?.Any(lv => lv.Name == name) ?? false;
    }

    private void ValidateMethodInClassOrParents(Guid invokeMethodId, Class attributeClass)
    {
        if(attributeClass.Methods != null && attributeClass.Methods.Any(m => m.Id == invokeMethodId))
        {
            return;
        }

        if(attributeClass.Parent != null)
        {
            Class? parentClass = classRepository.Get(c => c.Id == attributeClass.Parent.Id);
            if(parentClass != null)
            {
                ValidateMethodInClassOrParents(invokeMethodId, parentClass);
                return;
            }
        }

        throw new ArgumentException($"Method with id {invokeMethodId} not found");
    }

    private InvokeMethod CreateInvokeMethod(CreateInvokeMethodArgs invokeArg, Method method)
    {
        return invokeMethodService.CreateInvokeMethod(invokeArg, method);
    }

    #endregion

    #region SystemMethod

    public Method GetIdByName(string name)
    {
        var methods = methodRepository.GetAll(c => c.Id != Guid.Empty)?.ToList();
        var foundMethod = methods?.FirstOrDefault(cla => cla.Name == name);

        if(foundMethod == null)
        {
            throw new ArgumentException("Method not found");
        }

        return foundMethod;
    }

    #endregion

}
