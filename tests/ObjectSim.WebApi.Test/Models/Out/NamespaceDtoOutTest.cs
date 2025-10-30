using ObjectSim.Domain;
using ObjectSim.WebApi.DTOs.Out;

namespace ObjectSim.WebApi.Test.Models.Out;
[TestClass]
public class NamespaceDtoOutTest
{
    [TestMethod]
    public void FromEntity_ShouldMapPropertiesCorrectly()
    {

        var ns = new Namespace
        {
            Id = Guid.NewGuid(),
            Name = "Core",
            ParentId = Guid.NewGuid()
        };

        var dto = NamespaceInformationDtoOut.FromEntity(ns);
        Assert.AreEqual(ns.Id, dto.Id);
        Assert.AreEqual(ns.Name, dto.Name);
        Assert.AreEqual(ns.ParentId, dto.ParentId);
    }
    [TestMethod]
    public void FromEntity_WithNullParent_ShouldAllowNullParentId()
    {
        var ns = new Namespace
        {
            Id = Guid.NewGuid(),
            Name = "Root",
            ParentId = null
        };

        var dto = NamespaceInformationDtoOut.FromEntity(ns);

        Assert.IsNull(dto.ParentId);
    }
}
