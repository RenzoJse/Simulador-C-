using ObjectSim.WebApi.DTOs.Out;

namespace ObjectSim.WebApi.Test.Models.Out;
[TestClass]
public class AttributeModelOutTest
{
    [TestMethod]
    public void AttributeModelOut_ShouldCreateInstance()
    {
        var model = new AttributeDtoOut
        {
            Id = Guid.NewGuid(),
            Name = "TestName",
            DataTypeId = Guid.NewGuid(),
            Visibility = "Private",
            ClassId = Guid.NewGuid()
        };

        Assert.IsNotNull(model);
        Assert.AreEqual("TestName", model.Name);
        Assert.AreEqual("Private", model.Visibility);
        Assert.AreNotEqual(Guid.Empty, model.Id);
        Assert.AreNotEqual(Guid.Empty, model.DataTypeId);
        Assert.AreNotEqual(Guid.Empty, model.ClassId);
    }
}
