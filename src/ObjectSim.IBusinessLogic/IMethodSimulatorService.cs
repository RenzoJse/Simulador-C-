using ObjectSim.Domain.Args;

namespace ObjectSim.IBusinessLogic;
public interface IMethodSimulatorService
{
    string Simulate(SimulateExecutionArgs args);
}
