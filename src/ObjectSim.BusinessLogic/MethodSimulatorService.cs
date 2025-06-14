using ObjectSim.Abstractions;
using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.BusinessLogic;
public class MethodSimulatorService(IRepository<Method> methodRepository, IRepository<Class> classRepository,
    IOutputModelTransformerService outputModelTransformerService) : IMethodSimulatorService
{
    public string Simulate(SimulateExecutionArgs args)
    {
        ArgumentNullException.ThrowIfNull(args);

        var referenceType = GetClassById(args.ReferenceId); // vehiculo
        var instanceType = GetClassById(args.InstanceId); // Auto

        ValidateIsValidInstance(instanceType, referenceType);

        var method = GetMethodById(args.MethodId); // IniciarViaje
        if(!instanceType.Methods!.Contains(method))
        {
            if(!referenceType.Methods!.Contains(method))
            {
                throw new Exception("Method not found in this classes.");
            }
        }

        var result = "Execution: \n"
                     + instanceType.Name + "." + method.Name + "() -> " + instanceType.Name + "." + method.Name + "()\n";
        result += SimulateInternal(method, 0);

        SelectOutputModel("HtmlOutputModelTransformer");

        return outputModelTransformerService.TransformModel(result).ToString()!;
    }

    private void SelectOutputModel(string name)
    {
        outputModelTransformerService.SelectImplementation(name);
    }

    private void ValidateIsValidInstance(Class instanceType, Class referenceType)
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
        if (!visited.Add(method.Id))
        {
            return "";
        }

        var result = "";
        var indent = new string(' ', indentLevel * 5);

        foreach (var methodInvoke in method.MethodsInvoke)
        {
            var objMethodToInvoke = methodRepository.Get(m => m.Id == methodInvoke.MethodId);

            result += $"{indent}{methodInvoke.Reference}.{objMethodToInvoke!.Name}() -> ";

            if (objMethodToInvoke.MethodsInvoke.Count > 0)
            {
                result += SimulateInternal(objMethodToInvoke, indentLevel + 1, visited);
            }
        }

        return result;
    }

}
