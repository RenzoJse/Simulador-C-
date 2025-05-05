using FluentAssertions;
using ObjectSim.Domain.Args;

namespace ObjectSim.Domain.Test.Args;

[TestClass]
public class CreateDataTypeArgsTest
{
    [TestMethod]
    public void Properties_SetAndGet_ShouldReturnExpectedValues()
    {
        const string name = "TestType";
        const string type = "int";
        var args = new CreateDataTypeArgs(name, type);

        args.Name.Should().Be(name);
        args.Type.Should().Be(type);

        const string newName = "OtherType";
        const string newType = "float";
        args.Name = newName;
        args.Type = newType;

        args.Name.Should().Be(newName);
        args.Type.Should().Be(newType);
    }
}
