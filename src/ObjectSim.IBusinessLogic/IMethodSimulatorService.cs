using ObjectSim.Domain.Args;

namespace ObjectSim.IBusinessLogic;
public interface IMethodSimulatorService
{
    List<string> Simulate(SimulateExecutionArgs args);
}
