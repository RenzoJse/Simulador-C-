using ObjectSim.Domain;
using ObjectSim.WebApi.DTOs.Out;

namespace ObjectSim.WebApi.Test.Models.Out;
[TestClass]
public class NamespaceDtoOutTest
{
    [TestMethod]
    public void FromEntity_ShouldMapPropertiesCorrectly()
    {
        // Arrange
        var ns = new Namespace
        {
            Id = Guid.NewGuid(),
            Name = "Core",
            ParentId = Guid.NewGuid()
        };

        // Act
        var dto = NamespaceInformationDtoOut.FromEntity(ns);

        // Assert
        Assert.AreEqual(ns.Id, dto.Id);
        Assert.AreEqual(ns.Name, dto.Name);
        Assert.AreEqual(ns.ParentId, dto.ParentId);
    }
}
