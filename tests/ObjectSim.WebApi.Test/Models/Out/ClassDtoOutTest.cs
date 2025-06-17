using FluentAssertions;
using ObjectSim.Domain;
using ObjectSim.WebApi.DTOs.Out;

namespace ObjectSim.WebApi.Test.Models.Out;

[TestClass]
public class ClassDtoOutTest
{
    [TestMethod]
    public void Constructor_WithValidClassInfo_MapsPropertiesCorrectly()
    {
        var parent = new Class
        {
            Id = Guid.NewGuid(),
            Name = "ParentClass",
            IsAbstract = true,
            IsInterface = false,
            IsSealed = false,
            Attributes = new List<Domain.Attribute>(),
            Methods = new List<Method>(),
            Parent = null
        };

        var classInfo = new Class
        {
            Id = Guid.NewGuid(),
            Name = "MyClass",
            IsAbstract = false,
            IsInterface = true,
            IsSealed = true,
            Attributes = new List<Domain.Attribute> {  },
            Methods = new List<Method> { },
            Parent = parent
        };

        var dto = new ClassDtoOut(classInfo);

        dto.ClassInfo.Should().BeSameAs(classInfo);

        dto.Name.Should().Be("MyClass");
        dto.IsAbstract.Should().BeFalse();
        dto.IsInterface.Should().BeTrue();
        dto.IsSealed.Should().BeTrue();
        dto.Id.Should().Be(classInfo.Id);
        dto.Parent.Should().Be(parent.Id);

        dto.Attributes.Should().BeEmpty();
        dto.Methods.Should().BeEmpty();
    }

    [TestMethod]
    public void Constructor_WhenClassInfoIsNull_ThrowsNullReferenceException()
    {
        Action act = () =>
        {
            _ = new ClassDtoOut(null!);
        };
        act.Should().Throw<NullReferenceException>();
    }

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
