using ObjectSim.Domain.Args;

namespace ObjectSim.Domain.Test.Args;

[TestClass]
public class CreateInvokeMethodArgsTest
{
    private Guid randomId = Guid.NewGuid();
    
    [TestMethod]
    public void InvokeMethodId_SetAndGet_ShouldReturnSameValue()
    {
        var invokeMethodArgs = new CreateInvokeMethodArgs(randomId) { InvokeMethodId = randomId };

        Assert.AreEqual(randomId, invokeMethodArgs.InvokeMethodId);
    }
    
    [TestMethod]
    public void MethodId_SetAndGet_ShouldReturnSameValue()
    {
        var invokeMethodArgs = new CreateInvokeMethodArgs(randomId, randomId);
        
        Assert.AreEqual(randomId, invokeMethodArgs.InvokeMethodId);
    }
}
