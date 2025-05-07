using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.BusinessLogic;
public class MethodSimulatorService(IRepository<DataType> dataTypeRepository, IRepository<Method> methodRepository, IRepository<Class> classRepository) : IMethodSimulatorService
{
    public string Simulate(SimulateExecutionArgs args)
    {
        ArgumentNullException.ThrowIfNull(args);

        var referenceType = GetReferenceType(args.ReferenceType); // vehiculo
        var instanceType = GetReferenceType(args.InstanceType); // Auto

        if(referenceType != instanceType)
        {
            ValidateHierarchy(referenceType, instanceType);
        }

        var method = FindMethodById(referenceType, args.MethodId); //aca consigo el metodo IniciarViaje
        var result = "Execution: \n"
                     + instanceType.Type + "." + method.Name + "() -> " + instanceType.Type + "." + method.Name + "() -> \n";
        return result + SimulateInternal(method, 0);
    }

    private Method FindMethodById(DataType referenceType, Guid methodId)
    {
        var method = GetMethodById(methodId);
        ValidateMethodBelongsToClass(method, referenceType);
        return method;
    }

    private Method GetMethodById(Guid methodId)
    {
        var method = methodRepository.Get(m => m.Id == methodId);
        if (method == null)
        {
            throw new Exception("Method not found");
        }
        return method;
    }

    private void ValidateMethodBelongsToClass(Method method, DataType referenceType)
    {
        var classObj = classRepository.Get(c => c.Name == referenceType.Name);
        if (!classObj!.Methods!.Contains(method))
        {
            throw new Exception("Method not found in reference type");
        }
    }

    private DataType GetReferenceType(string typeName)
    {
        var dataType = dataTypeRepository.Get(dt => dt.Type == typeName);
        return dataType ?? throw new Exception($"Type '{typeName}' not found");
    }

    private void ValidateHierarchy(DataType reference, DataType instance)
    {
        var classObj = classRepository.Get(c => c.Name == instance.Name);
        if(classObj!.Parent != null)
        {
            var parentClass = classObj.Parent;
            if(parentClass.Name != reference.Type)
            {
                throw new Exception($"Parent class '{parentClass.Name}' not found in reference type '{reference.Type}'");
            }
        }
        else
        {
            throw new Exception($"Parent class '{instance.Name}' not found in reference type '{reference.Type}'");
        }
    }

    private string SimulateInternal(Method method, int indentLevel)
    {
        var result = "";

        var indent = new string(' ', indentLevel * 5);

        foreach (var methodInvoke in method.MethodsInvoke)
        {
            var objMethodToInvoke = methodRepository.Get(m => m.Id == methodInvoke.MethodId);

            result += $"{indent}{methodInvoke.Reference}.{objMethodToInvoke!.Name}() -> ";

            if (objMethodToInvoke.MethodsInvoke.Count > 0)
            {
                result += SimulateInternal(objMethodToInvoke, indentLevel + 1);
            }
        }

        return result;
    }
}
