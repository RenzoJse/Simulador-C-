using FluentAssertions;
using ObjectSim.Domain;
using ObjectSim.WebApi.DTOs.Out;

namespace ObjectSim.WebApi.Test.Models.Out;

[TestClass]
public class ClassDtoOutTest
{
    [TestMethod]
    public void Constructor_WhenParentIsNull_ParentPropertyIsNull()
    {
        var classInfo = new Class
        {
            Id = Guid.NewGuid(),
            Name = "SoloClass",
            IsAbstract = false,
            IsInterface = false,
            IsSealed = false,
            Attributes = new List<Domain.Attribute>(),
            Methods = new List<Method>(),
            Parent = null
        };

        var dto = new ClassDtoOut(classInfo);

        dto.Parent.Should().BeNull();
    }
}
