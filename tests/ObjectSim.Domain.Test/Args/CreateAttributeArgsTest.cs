using FluentAssertions;
using ObjectSim.Domain.Args;

namespace ObjectSim.Domain.Test.Args;

[TestClass]
public class CreateAttributeArgsTest
{
    [TestMethod]
    public void Properties_SetAndGet_ShouldReturnExpectedValues()
    {
        const string name = "TestAttribute";
        var dataTypeId = Guid.NewGuid();
        const string visibility = "public";
        var classId = Guid.NewGuid();
        var args = new CreateAttributeArgs(dataTypeId, visibility, classId, name);

        const string newName = "OtherAttribute";
        var newDataTypeId = Guid.NewGuid();
        const string newVisibility = "private";
        var newId = Guid.NewGuid();
        var newClassId = Guid.NewGuid();

        args.DataTypeId.Should().Be(dataTypeId);
        args.Visibility.Should().Be(visibility);
        args.ClassId.Should().Be(classId);
        args.Name.Should().Be(name);

        args.DataTypeId = newDataTypeId;
        args.Visibility = newVisibility;
        args.Id = newId;
        args.ClassId = newClassId;
        args.Name = newName;

        args.DataTypeId.Should().Be(newDataTypeId);
        args.Visibility.Should().Be(newVisibility);
        args.Id.Should().Be(newId);
        args.ClassId.Should().Be(newClassId);
        args.Name.Should().Be(newName);
    }

}
