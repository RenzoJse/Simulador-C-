using ObjectSim.Domain;
using ObjectSim.WebApi.DTOs.Out;
using ValueType = ObjectSim.Domain.ValueType;

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
            TypeId = Guid.NewGuid(),
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
        Assert.AreEqual("int", methodInfo.Type);
        Assert.AreEqual("Public", methodInfo.Accessibility);
        Assert.IsFalse(methodInfo.IsAbstract);
        Assert.IsFalse(methodInfo.IsSealed);
        Assert.IsFalse(methodInfo.IsOverride);
        Assert.IsNotNull(methodInfo.LocalVariables);
        Assert.IsNotNull(methodInfo.Parameters);
        Assert.IsNotNull(methodInfo.InvokeMethodsIds);
    }
}
