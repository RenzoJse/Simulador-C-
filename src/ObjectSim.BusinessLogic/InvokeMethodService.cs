using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.BusinessLogic;

public class InvokeMethodService(IRepository<InvokeMethod> invokeMethodRepository) : IInvokeMethodService
{

    #region CreateInvokeMethod

    public InvokeMethod CreateInvokeMethod(CreateInvokeMethodArgs invokeMethodArgs, Method method)
    {
        ArgumentNullException.ThrowIfNull(invokeMethodArgs);
        ArgumentNullException.ThrowIfNull(method);

        var newInvokeMethod = new InvokeMethod(method.Id, invokeMethodArgs.InvokeMethodId, invokeMethodArgs.Reference);
        AddInvokeMethodToRepository(newInvokeMethod);
        AddInvokeMethodToMethod(newInvokeMethod, method);
        return newInvokeMethod;
    }

    private void AddInvokeMethodToRepository(InvokeMethod invokeMethod)
    {
        invokeMethodRepository.Add(invokeMethod);
    }

    private static void AddInvokeMethodToMethod(InvokeMethod invokeMethod, Method method)
    {
        method.MethodsInvoke.Add(invokeMethod);
    }

    #endregion

}
