using ObjectSim.Domain.Args;

namespace ObjectSim.IBusinessLogic;
public interface IMethodSimulatorService
{
    object Simulate(SimulateExecutionArgs args);
}
