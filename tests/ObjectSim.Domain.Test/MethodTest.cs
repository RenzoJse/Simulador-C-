using FluentAssertions;

namespace ObjectSim.Domain.Test;

[TestClass]
public class MethodTest
{
    [TestMethod]
    public void DataType_Property_SetAndGet_ShouldBeEqual()
    {
        var method = new Method();
        method.DataType = Method.MethodDataType.Decimal;
        method.DataType.Should().Be(Method.MethodDataType.Decimal);
    }

    [TestMethod]
    public void Visibility_Property_SetAndGet_ShouldBeEqual()
    {
        var method = new Method();

        method.Visibility = Method.MethodVisibility.ProtectedInternal;

        method.Visibility.Should().Be(Method.MethodVisibility.ProtectedInternal);
    }

    [TestMethod]
    public void MethodDataTypeCreateMethod_OKTest()
    {
        var method = new Method();
        method.DataType = Method.MethodDataType.String;
        Assert.AreEqual(Attribute.MethodDataType.String, method.DataType);
    }
    [TestMethod]
    public void MethodVisibilityCreateMethod_OKTest()
    {
        var method = new Method();
        method.Visibility = Method.MethodVisibility.Public;
        Assert.AreEqual(Method.MethodVisibility.Public, method.Visibility);
    }
}
