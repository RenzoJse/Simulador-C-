using ObjectSim.Domain.Args;

namespace ObjectSim.Domain.Test.Args;

[TestClass]
public class CreateInvokeMethodArgsTest
{
    private readonly Guid _randomId = Guid.NewGuid();
    
    [TestMethod]
    public void InvokeMethodId_SetAndGet_ShouldReturnSameValue()
    {
        var invokeMethodArgs = new CreateInvokeMethodArgs(_randomId, _randomId, "test") { InvokeMethodId = _randomId };

        Assert.AreEqual(_randomId, invokeMethodArgs.InvokeMethodId);
    }
    
    [TestMethod]
    public void MethodId_SetAndGet_ShouldReturnSameValue()
    {
        var methodId = Guid.NewGuid();
        var invokeMethodArgs = new CreateInvokeMethodArgs(_randomId, methodId, "test") { MethodId = methodId };
        
        Assert.AreEqual(methodId, invokeMethodArgs.MethodId);
    }

    [TestMethod]
    public void Reference_SetAndGet_ShouldReturnSameValue()
    {
        const string reference = "this";
        var invokeMethodArgs = new CreateInvokeMethodArgs(_randomId, _randomId, reference) { Reference = reference };
        
        Assert.AreEqual(reference, invokeMethodArgs.Reference);
    }
}
