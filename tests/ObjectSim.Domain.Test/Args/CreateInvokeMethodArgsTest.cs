using ObjectSim.Domain.Args;

namespace ObjectSim.Domain.Test.Args;

[TestClass]
public class CreateInvokeMethodArgsTest
{
    [TestMethod]
    public void InvokeMethodId_SetAndGet_ShouldReturnSameValue()
    {
        var invokeMethodArgs = new CreateInvokeMethodArgs(Guid.NewGuid());
        var expectedId = Guid.NewGuid();
        
        invokeMethodArgs.InvokeMethodId = expectedId;
        
        Assert.AreEqual(expectedId, invokeMethodArgs.InvokeMethodId);
    }
}
