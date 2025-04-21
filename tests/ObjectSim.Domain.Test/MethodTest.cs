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
}
