using ObjectSim.Domain.Args;

namespace ObjectSim.IBusinessLogic;
public interface IMethodSimulationService
{
    List<string> Simulate(SimulateExecutionArgs args);
}
