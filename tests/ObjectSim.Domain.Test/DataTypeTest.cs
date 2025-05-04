using FluentAssertions;

namespace ObjectSim.Domain.Test;

[TestClass]
public class DataTypeTest
{
    [TestMethod]
    public void ValueType_ShouldInitializeId()
    {
        var valueType = new ValueType("ValidName", "int", []);

        valueType.Id.Should().NotBeEmpty();
    }

    [TestMethod]
    public void IsSameType_TypesAreEqual_ReturnsTrue()
    {
        var type1 = new ValueType("variable", "int", []);
        var type2 = new ValueType("variable", "int", []);

        var result = type1.IsSameType(type2);

        result.Should().BeTrue();
    }

    [TestMethod]
    public void IsSameType_TypesAreDifferent_ReturnsFalse()
    {
        var type1 =  new ValueType("variable", "int", []);
        var type2 =  new ValueType("variable", "bool", []);

        var result = type1.IsSameType(type2);

        result.Should().BeFalse();
    }
}
