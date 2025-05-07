using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.BusinessLogic;
public class MethodSimulatorService(IRepository<DataType> dataTypeRepository, IRepository<Method> methodRepository) : IMethodSimulatorService
{
    public List<string> Simulate(SimulateExecutionArgs args)
    {
        ArgumentNullException.ThrowIfNull(args);

        var referenceType = GetReferenceType(args.ReferenceType);
        var instanceType = GetReferenceType(args.InstanceType); // hasta aca es Auto, Auto

        ValidateHierarchy(referenceType, instanceType);

        var method = FindMethodByName(referenceType, args.MethodName); //aca consigo el metodo IniciarViaje

        return SimulateInternal(method.MethodsInvoke);
    }

    private Method FindMethodByName(DataType referenceType, string methodName)
    {
        return referenceType.MethodIds
                   .Select(id => methodRepository.Get(m => m.Id == id))
                   .FirstOrDefault(m => m?.Name != null &&
                                        string.Equals(m.Name, methodName, StringComparison.OrdinalIgnoreCase))
               ?? throw new Exception("Method not found in reference type");
    }

    private DataType GetReferenceType(string typeName)
    {
        var dataType = dataTypeRepository.Get(dt => dt.Type == typeName);
        return dataType ?? throw new Exception($"Type '{typeName}' not found");
    }

    private void ValidateHierarchy(DataType reference, DataType instance)
    {
        if(reference.Type != instance.Type && reference.Name != instance.Name)
        {
            throw new Exception($"'{instance.Type}' is not a valid subtype of '{reference.Type}'");
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
