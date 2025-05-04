using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.BusinessLogic;
public class MethodSimulatorService(IRepository<Class> classRepository, IRepository<Method> methodRepository) : IMethodSimulatorService
{
    public List<string> Simulate(SimulateExecutionArgs args)
    {
        if(args is null)
        {
            throw new ArgumentNullException(nameof(args), "Arguments cannot be null");
        }

        var referenceClass = classRepository.Get(c => c.Id == args.ReferenceClassId)
        ?? throw new ArgumentException("Reference class not found.");

        var instanceClass = classRepository.Get(c => c.Id == args.InstanceClassId)
            ?? throw new ArgumentException("Instance class not found.");

        var method = methodRepository.Get(m => m.Id == args.MethodToExecuteId)
            ?? throw new ArgumentException("Method to execute not found.");

        var trace = new List<string>();

        if(referenceClass?.Name == null)
        {
            throw new InvalidOperationException("Reference class must have a valid name.");
        }

        if(instanceClass?.Name == null)
        {
            throw new InvalidOperationException("Instance class must have a valid name.");
        }

        var className = referenceClass.Name;

        SimulateMethod(referenceClass.Name, method, trace, "this.");

        return trace;
    }

    private void SimulateMethod(string className, Method method, List<string> trace, string prefix)
    {
        var call = $"{className}.{prefix}{method.Name}()";
        trace.Add(call);

        foreach(var subMethod in method.MethodsInvoke)
        {
            var subClassName = classRepository.Get(c => c.Id == subMethod.ClassId)?.Name ?? "UnknownClass";
            SimulateMethod(subClassName, subMethod, trace, prefix);
        }
    }

}
