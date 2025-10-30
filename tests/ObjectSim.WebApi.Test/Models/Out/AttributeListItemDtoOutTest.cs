using ObjectSim.WebApi.DTOs.Out;
using AttributeDomain = ObjectSim.Domain.Attribute;
namespace ObjectSim.WebApi.Test.Models.Out;
[TestClass]
public class AttributeListItemDtoOutTest
{
    [TestMethod]
    public void FromEntity_ShouldMapCorrectly()
    {
        var id = Guid.NewGuid();
        var attribute = new AttributeDomain
        {
            Id = id,
            Name = "ExampleAttribute",
            DataTypeId = Guid.NewGuid(),
            ClassId = Guid.NewGuid(),
            Visibility = AttributeDomain.AttributeVisibility.Public
        };

        var dto = AttributeListItemDtoOut.FromEntity(attribute);

        Assert.IsNotNull(dto);
        Assert.AreEqual(id, dto.Id);
        Assert.AreEqual("ExampleAttribute", dto.Name);
    }
    [TestMethod]
    public void AttributeListItemDtoOut_ShouldCreateInstanceCorrectly()
    {
        var id = Guid.NewGuid();

        var dto = new AttributeListItemDtoOut
        {
            Id = id,
            Name = "TestName"
        };

        Assert.IsNotNull(dto);
        Assert.AreEqual(id, dto.Id);
        Assert.AreEqual("TestName", dto.Name);
    }
}
