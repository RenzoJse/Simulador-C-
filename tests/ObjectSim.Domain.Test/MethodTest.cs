using FluentAssertions;

namespace ObjectSim.Domain.Test;

[TestClass]
public class MethodTest
{
    [TestMethod]
    public void DataType_Property_SetAndGet_ShouldBeEqual()
    {
        var method = new Method();
        method.Type = Method.MethodDataType.Decimal;
        method.Type.Should().Be(Method.MethodDataType.Decimal);
    }

    [TestMethod]
    public void Visibility_Property_SetAndGet_ShouldBeEqual()
    {
        var method = new Method();

        method.Accessibility = Method.MethodAccessibility.ProtectedInternal;

        method.Accessibility.Should().Be(Method.MethodAccessibility.ProtectedInternal);
    }

    [TestMethod]
    public void MethodDataTypeCreateMethod_OKTest()
    {
        var method = new Method();
        method.Type = Method.MethodDataType.String;
        Assert.AreEqual(Method.MethodDataType.String, method.Type);
    }
    [TestMethod]
    public void MethodVisibilityCreateMethod_OKTest()
    {
        var method = new Method();
        method.Accessibility = Method.MethodAccessibility.Public;
        Assert.AreEqual(Method.MethodAccessibility.Public, method.Accessibility);
    }

    [TestMethod]
    public void Name_Property_SetAndGet_ShouldBeEqual()
    {
        var method = new Method();
        method.Name = "TestMethod";
        method.Name.Should().Be("TestMethod");
    }

    [TestMethod]
    public void Id_Property_SetAndGet_ShouldBeEqual()
    {
        var id = Guid.NewGuid();
        var method = new Method();
        method.Id = id;
        method.Id.Should().Be(id);
    }
}
