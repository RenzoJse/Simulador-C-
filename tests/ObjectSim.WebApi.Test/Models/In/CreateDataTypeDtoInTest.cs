using FluentAssertions;
using ObjectSim.WebApi.DTOs.In;

namespace ObjectSim.WebApi.Test.Models.In;

[TestClass]
public class CreateDataTypeDtoInTest
{
    [TestMethod]
    public void CreateDataTypeDtoIn_WithArguments_ReturnsNewInstance()
    {
        var model = new CreateDataTypeDtoIn { classId = "TestName", Type = "int" };

        model.classId.Should().Be("TestName");
        model.Type.Should().Be("int");
    }

    [TestMethod]
    public void CreateDataTypeDtoInToArgs_WithArguments_DataTypeArgs()
    {
        var model = new CreateDataTypeDtoIn { classId = "TestName", Type = "int" };
        var args = model.ToArgs();
        args.Should().NotBeNull();
        args.ClassId.Should().Be("TestName");
        args.Type.Should().Be("int");
    }
}
