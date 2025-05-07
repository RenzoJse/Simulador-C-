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
            DataTypeName = "string",
            DataTypeKind = "ReferenceId",
            Visibility = "Private",
            ClassId = Guid.NewGuid()
        };

        Assert.IsNotNull(model);
        Assert.AreEqual("TestName", model.Name);
        Assert.AreEqual("string", model.DataTypeName);
        Assert.AreEqual("ReferenceId", model.DataTypeKind);
        Assert.AreEqual("Private", model.Visibility);
        Assert.AreNotEqual(Guid.Empty, model.Id);
        Assert.AreNotEqual(Guid.Empty, model.ClassId);
    }
}
