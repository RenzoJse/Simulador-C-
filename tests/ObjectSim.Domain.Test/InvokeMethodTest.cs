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
    
    [TestMethod]
    public void Constructor_InitializesProperties_WithCorrectValues()
    {
        var methodId = Guid.NewGuid();
        var invokeMethodId = Guid.NewGuid();
        const string reference = "TestReference";
        
        var invokeMethod = new InvokeMethod(methodId, invokeMethodId, reference);
        
        invokeMethod.MethodId.Should().Be(methodId);
        invokeMethod.InvokeMethodId.Should().Be(invokeMethodId);
        invokeMethod.Reference.Should().Be(reference);
    }
}
