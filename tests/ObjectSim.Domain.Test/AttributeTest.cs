namespace ObjectSim.Domain.Test;
[TestClass]
public class AttributeTest
{
    [TestMethod]
    public void AttributeDataTypeTest()
    {
        var attribute = new Attribute();
        attribute.DataType = Attribute.AttributeDataType.String;
        Assert.AreEqual(Attribute.AttributeDataType.String, attribute.DataType);
    }
    [TestMethod]
    public void AttributeVisibilityTest()
    {
        var attribute = new Attribute();
        attribute.Visibility = Attribute.AttributeVisibility.Public;
        Assert.AreEqual(Attribute.AttributeVisibility.Public, attribute.Visibility);
    }
    [TestMethod]
    public void AttributeIdTest()
    {
        var attribute = new Attribute();
        attribute.Id = 1;
        Assert.AreEqual(1, attribute.Id);
    }
    [TestMethod]
    public void AttributeNameTest()
    {
        var attribute = new Attribute();
        attribute.Name = "TestAttribute";
        Assert.AreEqual("TestAttribute", attribute.Name);
    }
}
