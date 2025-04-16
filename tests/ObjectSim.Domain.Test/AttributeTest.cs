using FluentAssertions;

namespace ObjectSim.Domain.Test;
[TestClass]
public class AttributeTest
{
    [TestMethod]
    public void AttributeDataTypeCreateAttribute_OKTest()
    {
        var attribute = new Attribute();
        attribute.DataType = Attribute.AttributeDataType.String;
        Assert.AreEqual(Attribute.AttributeDataType.String, attribute.DataType);
    }
    [TestMethod]
    public void AttributeVisibilityCreateAttribute_OKTest()
    {
        var attribute = new Attribute();
        attribute.Visibility = Attribute.AttributeVisibility.Public;
        Assert.AreEqual(Attribute.AttributeVisibility.Public, attribute.Visibility);
    }
    [TestMethod]
    public void AttributeName_CreateAttribute_OKTest()
    {
        var attribute = new Attribute();
        attribute.Name = "TestAttribute";
        Assert.AreEqual("TestAttribute", attribute.Name);
    }
    [TestMethod]
    public void Validate_ValidAttribute_ShouldNotThrow()
    {
        var attribute = new Attribute
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            DataType = Attribute.AttributeDataType.String,
            Visibility = Attribute.AttributeVisibility.Public
        };

        Action act = attribute.Validate;

        act.Should().NotThrow();
    }
}
