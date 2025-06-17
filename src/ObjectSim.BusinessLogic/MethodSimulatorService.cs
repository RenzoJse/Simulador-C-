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
        if (!instanceType.Methods!.Contains(method) && !referenceType.Methods!.Contains(method))
        {
            throw new Exception("Method not found in this classes.");
        }

        var mainParameters = FormatParameters(method);
        var localVariables = FormatLocalVariables(method);

        var result = $"Execution: \n{instanceType.Name}.{method.Name}({mainParameters}) -> {instanceType.Name}.{method.Name}({mainParameters})\n";
        if (!string.IsNullOrEmpty(localVariables))
        {
            result += localVariables + "\n";
        }
        result += SimulateInternal(method, 0);

        SelectOutputModel(args.OutputModelName!);

        return outputModelTransformerService.TransformModel(result);
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

    private string SimulateInternal(Method method, int indentLevel, HashSet<Guid>? visited = null)
    {
        visited ??= [];
        if (!AddToVisited(visited, method.Id))
        {
            return string.Empty;
        }

        var result = string.Empty;
        var indent = GetIndentation(indentLevel);

        foreach (var methodInvoke in method.MethodsInvoke)
        {
            var objMethodToInvoke = GetMethodById(methodInvoke.MethodId);
            var parameters = FormatParameters(objMethodToInvoke);
            result += $"{indent}{methodInvoke.Reference}.{objMethodToInvoke.Name}({parameters}) -> ";

            if (HasMethodInvocations(objMethodToInvoke))
            {
                result += SimulateInternal(objMethodToInvoke, indentLevel + 1, visited);
            }
        }

        return result;
    }

    private static bool AddToVisited(HashSet<Guid> visited, Guid methodId)
    {
        return visited.Add(methodId);
    }

    private static string GetIndentation(int indentLevel)
    {
        return new string(' ', indentLevel * 5);
    }

    private static string FormatParameters(Method method)
    {
        return method.Parameters.Count == 0 ? string.Empty : string.Join(", ", method.Parameters.Select(p => $"{Capitalize(p.Name)} {p.Name}"));
    }

    private static bool HasMethodInvocations(Method method)
    {
        return method.MethodsInvoke.Count > 0;
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
