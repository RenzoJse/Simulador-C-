using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.BusinessLogic;
public class MethodSimulatorService(IRepository<DataType> dataTypeRepository, IRepository<Method> methodRepository, IRepository<Class> classRepository) : IMethodSimulatorService
{
    public List<string> Simulate(SimulateExecutionArgs args)
    {
        ArgumentNullException.ThrowIfNull(args);

        var referenceType = GetReferenceType(args.ReferenceType); // vehiculo
        var instanceType = GetReferenceType(args.InstanceType); // Auto

        if(referenceType != instanceType)
        {
            ValidateHierarchy(referenceType, instanceType);
        }

        var method = FindMethodById(referenceType, args.MethodId); //aca consigo el metodo IniciarViaje

        return SimulateInternal(method.MethodsInvoke);
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

    private List<string> SimulateInternal(List<InvokeMethod> methodsToInvoke)
    {
        var result = new List<string>();

        foreach(var invoked in methodsToInvoke)
        {
            var methodToInvoke = methodRepository.Get(m => m.Id == invoked.MethodId);
            result.Add($"{invoked.Reference}.{methodToInvoke!.Name}() ->");
        }

        return result;
    }
}
