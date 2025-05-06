using ObjectSim.Domain.Args;

namespace ObjectSim.Domain.Test.Args;

[TestClass]
public class CreateInvokeMethodArgsTest
{
    private readonly Guid _randomId = Guid.NewGuid();

    [TestMethod]
    public void InvokeMethodId_SetAndGet_ShouldReturnSameValue()
    {
        var invokeMethodArgs = new CreateInvokeMethodArgs(_randomId, "test") { InvokeMethodId = _randomId };

        Assert.AreEqual(_randomId, invokeMethodArgs.InvokeMethodId);
    }

    [TestMethod]
    public void Reference_SetAndGet_ShouldReturnSameValue()
    {
        const string reference = "this";
        var invokeMethodArgs = new CreateInvokeMethodArgs(_randomId, reference) { Reference = reference };

        Assert.AreEqual(reference, invokeMethodArgs.Reference);
    }
}
