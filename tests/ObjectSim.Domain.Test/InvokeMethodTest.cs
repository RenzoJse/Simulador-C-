using FluentAssertions;

namespace ObjectSim.Domain.Test;

[TestClass]
public class InvokeMethodTest
{
    [TestMethod]
    public void MethodId_SetAndGet_ShouldReturnExpectedValue()
    {
        var invokeMethod = new InvokeMethod(Guid.NewGuid(), Guid.NewGuid(), "Test");
        var newId = Guid.NewGuid();
        
        invokeMethod.MethodId = newId;
        
        invokeMethod.MethodId.Should().Be(newId);
    }
    
    [TestMethod]
    public void InvokeMethodId_SetAndGet_ShouldReturnExpectedValue()
    {
        var invokeMethod = new InvokeMethod(Guid.NewGuid(), Guid.NewGuid(), "Test");
        var newId = Guid.NewGuid();
        
        invokeMethod.InvokeMethodId = newId;
     
        invokeMethod.InvokeMethodId.Should().Be(newId);
    }
    
    [TestMethod]
    public void Reference_SetAndGet_ShouldReturnExpectedValue()
    {
        var invokeMethod = new InvokeMethod(Guid.NewGuid(), Guid.NewGuid(), "Test");
        const string newReference = "NewReference";
        
        invokeMethod.Reference = newReference;
        
        invokeMethod.Reference.Should().Be(newReference);
    }
}
