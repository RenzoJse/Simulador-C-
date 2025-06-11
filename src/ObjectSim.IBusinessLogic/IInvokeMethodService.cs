using ObjectSim.Domain;
using ObjectSim.Domain.Args;

namespace ObjectSim.IBusinessLogic;

public interface IInvokeMethodService
{
    public InvokeMethod CreateInvokeMethod(CreateInvokeMethodArgs invokeMethodArgs, Method method);
}

