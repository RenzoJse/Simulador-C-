using ObjectSim.Domain.Args;

namespace ObjectSim.Domain.Test.Args;

[TestClass]
public class CreateInvokeMethodArgsTest
{
    private readonly Guid _randomId = Guid.NewGuid();
    
    [TestMethod]
    public void InvokeMethodId_SetAndGet_ShouldReturnSameValue()
    {
        var invokeMethodArgs = new CreateInvokeMethodArgs(_randomId, _randomId) { InvokeMethodId = _randomId };

        Assert.AreEqual(_randomId, invokeMethodArgs.InvokeMethodId);
    }
    
    [TestMethod]
    public void MethodId_SetAndGet_ShouldReturnSameValue()
    {
        var methodId = Guid.NewGuid();
        var invokeMethodArgs = new CreateInvokeMethodArgs(_randomId, methodId);
        
        Assert.AreEqual(methodId, invokeMethodArgs.MethodId);
    }
}
