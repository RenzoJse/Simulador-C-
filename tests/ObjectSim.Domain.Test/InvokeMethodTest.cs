using FluentAssertions;

namespace ObjectSim.Domain.Test;

[TestClass]
public class InvokeMethodTest
{
    [TestMethod]
    public void MethodId_SetAndGet_ShouldReturnExpectedValue()
    {
        var invokeMethod = new InvokeMethod(Guid.NewGuid());
        var newId = Guid.NewGuid();
        
        invokeMethod.MethodId = newId;
        
        invokeMethod.MethodId.Should().Be(newId);
    }
}
