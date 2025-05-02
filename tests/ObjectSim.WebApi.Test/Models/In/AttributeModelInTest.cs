using ObjectSim.WebApi.DTOs.In;

namespace ObjectSim.WebApi.Test.Models.In;
[TestClass]
public class AttributeModelInTest
{
    [TestMethod]
    public void AttributeModelIn_ShouldCreateInstance()
    {
        var model = new CreateAttributeDtoIn
        {
            Name = "TestName",
            DataTypeName = "int",
            DataTypeKind = "ValueType",
            Visibility = "Public",
            ClassId = Guid.NewGuid()
        };

        Assert.IsNotNull(model);
        Assert.AreEqual("TestName", model.Name);
        Assert.AreEqual("int", model.DataTypeName);
        Assert.AreEqual("ValueType", model.DataTypeKind);
        Assert.AreEqual("Public", model.Visibility);
        Assert.AreNotEqual(Guid.Empty, model.ClassId);
    }
}
