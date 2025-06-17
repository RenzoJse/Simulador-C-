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
            Attributes = [],
            Methods = [],
            Parent = null
        };

        var dto = new ClassDtoOut()
        {
            Id = classInfo.Id,
            Name = classInfo.Name,
            IsAbstract = classInfo.IsAbstract,
            IsInterface = classInfo.IsInterface,
            IsSealed = classInfo.IsSealed,
            Attributes = classInfo.Attributes.Select(a => a.Name).ToList()!,
            Methods = classInfo.Methods.Select(m => m.Name).ToList()!,
            Parent = classInfo.Parent?.Id
        };;

        dto.Parent.Should().BeNull();
    }

    [TestMethod]
    public void ToInfo_MapsAllPropertiesCorrectly()
    {
        var parentId = Guid.NewGuid();
        var classInfo = new Class
        {
            Id = Guid.NewGuid(),
            Name = "TestClass",
            IsAbstract = true,
            IsInterface = false,
            IsSealed = true,
            Attributes = [],
            Methods = [new Method { Name = "method1" }, new Method { Name = "method2" }],
            Parent = new Class { Id = parentId }
        };

        var dto = ClassDtoOut.ToInfo(classInfo);

        dto.Id.Should().Be(classInfo.Id);
        dto.Name.Should().Be("TestClass");
        dto.IsAbstract.Should().BeTrue();
        dto.IsInterface.Should().BeFalse();
        dto.IsSealed.Should().BeTrue();
        dto.Attributes.Should().BeEquivalentTo([]);
        dto.Methods.Should().BeEquivalentTo(["method1", "method2"]);
        dto.Parent.Should().Be(parentId);
    }

}
