using ObjectSim.Domain;
using ObjectSim.Domain.Args;

namespace ObjectSim.IBusinessLogic;

public interface IMethodServiceCreate
{
    Method CreateMethod(CreateMethodArgs method);
}
