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
        var dataType = new CreateDataTypeArgs(name, "int");
        const string visibility = "public";
        var classId = Guid.NewGuid();
        var args = new CreateAttributeArgs(dataType, visibility, classId, name);

        const string newName = "OtherAttribute";
        var newDataType = new CreateDataTypeArgs(newName, "int");
        const string newVisibility = "private";
        var newId = Guid.NewGuid();
        var newClassId = Guid.NewGuid();

        args.DataType.Should().Be(dataType);
        args.Visibility.Should().Be(visibility);
        args.ClassId.Should().Be(classId);
        args.Name.Should().Be(name);

        args.DataType = newDataType;
        args.Visibility = newVisibility;
        args.Id = newId;
        args.ClassId = newClassId;
        args.Name = newName;

        args.DataType.Should().Be(newDataType);
        args.Visibility.Should().Be(newVisibility);
        args.Id.Should().Be(newId);
        args.ClassId.Should().Be(newClassId);
        args.Name.Should().Be(newName);
    }

}
