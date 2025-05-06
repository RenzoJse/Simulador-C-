using FluentAssertions;

namespace ObjectSim.Domain.Test;

[TestClass]
public class InvokeMethodTest
{
    [TestMethod]
    public void MethodId_SetAndGet_ShouldReturnExpectedValue()
    {
        var invokeMethod = new InvokeMethod(Guid.NewGuid(), Guid.NewGuid());
        var newId = Guid.NewGuid();
        
        invokeMethod.MethodId = newId;
        
        invokeMethod.MethodId.Should().Be(newId);
    }
    
    [TestMethod]
    public void InvokeMethodId_SetAndGet_ShouldReturnExpectedValue()
    {
        var invokeMethod = new InvokeMethod(Guid.NewGuid(), Guid.NewGuid());
        var newId = Guid.NewGuid();
        
        InvokeMethod.InvokeMethodId = newId;
     
        invokeMethod.InvokeMethodId.Should().Be(newId);
    }
}
