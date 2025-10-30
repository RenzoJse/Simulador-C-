using FluentAssertions;
using ObjectSim.Domain.Args;

namespace ObjectSim.Domain.Test.Args;

[TestClass]
public class CreateDataTypeArgsTest
{
    [TestMethod]
    public void Properties_SetAndGet_ShouldReturnExpectedValues()
    {
        var classId = Guid.NewGuid();
        const string type = "int";
        var args = new CreateDataTypeArgs(classId, type);

        args.Type.Should().Be(type);
        const string newType = "float";

        args.ClassId = classId;
        args.Type = newType;

        args.ClassId.Should().Be(classId);
        args.Type.Should().Be(newType);
    }
}
