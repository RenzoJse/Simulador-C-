using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.BusinessLogic;
public class MethodSimulatorService(IRepository<Method> methodRepository, IRepository<Class> classRepository,
    IOutputModelTransformerService outputModelTransformerService) : IMethodSimulatorService
{
    public object Simulate(SimulateExecutionArgs args)
    {
        ArgumentNullException.ThrowIfNull(args);

        var referenceType = GetClassById(args.ReferenceId); // vehiculo
        var instanceType = GetClassById(args.InstanceId); // Auto

        ValidateIsValidInstance(instanceType, referenceType);

        var method = GetMethodById(args.MethodId); // IniciarViaje
        ValidateMethodExists(method, instanceType, referenceType);

        var mainParameters = FormatParameters(method);
        var localVariables = FormatLocalVariables(method);

        var result = BuildExecutionHeader(instanceType, method, mainParameters);
        if (!string.IsNullOrEmpty(localVariables))
        {
            result += localVariables + "\n";
        }
        result += SimulateInternal(method, 0);

        SelectOutputModel(args.OutputModelName!);

        return outputModelTransformerService.TransformModel(result);
    }

    private static void ValidateMethodExists(Method method, Class instanceType, Class referenceType)
    {
        if (!instanceType.Methods!.Contains(method) && !referenceType.Methods!.Contains(method))
        {
            throw new Exception("Method not found in this classes.");
        }
    }

    private static string BuildExecutionHeader(Class instanceType, Method method, string mainParameters)
    {
        return $"Execution: \n{instanceType.Name}.{method.Name}({mainParameters}) -> {instanceType.Name}.{method.Name}({mainParameters})\n";
    }

    private static string FormatLocalVariables(Method method)
    {
        return method.LocalVariables.Count == 0 ? string.Empty : string.Join(" ", method.LocalVariables.Select(v => $"{Capitalize(v.Name)} {v.Name};"));
    }

    private void SelectOutputModel(string name)
    {
        outputModelTransformerService.SelectImplementation(name);
    }

    private static void ValidateIsValidInstance(Class instanceType, Class referenceType)
    {
        if(instanceType.Parent != referenceType && instanceType.Id != referenceType.Id)
        {
            throw new Exception("Invalid instance type.");
        }
    }

    private Class GetClassById(Guid classId)
    {
        var classObj = classRepository.Get(c => c.Id == classId);
        if(classObj == null)
        {
            throw new Exception("Class not found.");
        }
        return classObj;
    }

    private Method GetMethodById(Guid methodId)
    {
        var method = methodRepository.Get(m => m.Id == methodId);
        if(method == null)
        {
            throw new Exception("Method not found.");
        }
        return method;
    }

    private string SimulateInternal(Method method, int indentLevel)
    {
        var result = string.Empty;
        var indent = GetIndentation(indentLevel);

        foreach (var methodInvoke in method.MethodsInvoke)
        {
            var objMethodToInvoke = GetMethodById(methodInvoke.InvokeMethodId);
            var parameters = FormatParameters(objMethodToInvoke);
            result += $"{indent}{methodInvoke.Reference}.{objMethodToInvoke.Name}({parameters}) -> ";

            if (objMethodToInvoke.MethodsInvoke.Count > 0)
            {
                var firstInvoke = objMethodToInvoke.MethodsInvoke[0];
                var firstMethod = GetMethodById(firstInvoke.MethodId);
                result += SimulateInternal(firstMethod, indentLevel + 1);
            }
        }

        result = RemoveTrailingArrow(result);
        return result;
    }

    private static string RemoveTrailingArrow(string value)
    {
        return value.EndsWith(" -> ") ? value[..^4] : value;
    }

    private static string GetIndentation(int indentLevel)
    {
        return new string(' ', indentLevel * 5);
    }

    private static string FormatParameters(Method method)
    {
        return method.Parameters.Count == 0 ? string.Empty : string.Join(", ", method.Parameters.Select(p => $"{Capitalize(p.Name)} {p.Name}"));
    }

    private static string Capitalize(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }

        return char.ToUpper(value[0]) + value[1..];
    }

}
