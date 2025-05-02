using ObjectSim.Domain;
using ObjectSim.WebApi.DTOs.Out;

namespace ObjectSim.WebApi.Test.Models.Out;

[TestClass]
public class MethodInformationDtoOutTest
{
    [TestMethod]
    public void MethodInformationDtoOut_ShouldCreateInstance()
    {
        var method = new Method()
        {
            Name = "TestMethod",
            Type = Method.MethodDataType.Bool,
            Accessibility = Method.MethodAccessibility.Public,
            Abstract = false,
            IsSealed = false,
            IsOverride = false,
            LocalVariables = [],
            Parameters = [],
            MethodsInvoke = []
        };
            
        var methodInfo = new MethodInformationDtoOut(method);
        
        Assert.IsNotNull(methodInfo);
        Assert.AreEqual("TestMethod", methodInfo.Name);
        Assert.AreEqual("Bool", methodInfo.Type);
        Assert.AreEqual("Public", methodInfo.Accessibility);
        Assert.IsFalse(methodInfo.IsAbstract);
        Assert.IsFalse(methodInfo.IsSealed);
        Assert.IsFalse(methodInfo.IsOverride);
        Assert.IsNotNull(methodInfo.LocalVariables);
        Assert.IsNotNull(methodInfo.Parameters);
        Assert.IsNotNull(methodInfo.Methods);
    }
}
