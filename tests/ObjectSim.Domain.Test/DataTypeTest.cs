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
}
