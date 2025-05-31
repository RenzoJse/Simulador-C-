using ObjectSim.WebApi.DTOs.In;

namespace ObjectSim.WebApi.Test.Models.In;
[TestClass]
public class AttributeModelInTest
{
    [TestMethod]
    public void AttributeModelIn_ShouldCreateInstance()
    {
        var dataTypeId = Guid.NewGuid();
        var model = new CreateAttributeDtoIn
        {
            Name = "TestName",
            DataTypeId = dataTypeId.ToString(),
            Visibility = "Public",
            ClassId = Guid.NewGuid()
        };

        Assert.IsNotNull(model);
        Assert.AreEqual("TestName", model.Name);
        Assert.AreEqual(dataTypeId.ToString(), model.DataTypeId);
        Assert.AreEqual("Public", model.Visibility);
        Assert.AreNotEqual(Guid.Empty, model.ClassId);
    }
}
