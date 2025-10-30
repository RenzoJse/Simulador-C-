using FluentAssertions;
using ObjectSim.WebApi.DTOs.In;

namespace ObjectSim.WebApi.Test.Models.In;
[TestClass]
public class CreateVariableDtoInTest
{
    [TestMethod]
    public void ToArgs_WithValidTypeId_ReturnsCorrectArgs()
    {
        var guid = Guid.NewGuid();
        var name = "MyVariable";
        var dto = new CreateVariableDtoIn
        {
            TypeId = guid.ToString(),
            Name = name
        };

        var args = dto.ToArgs();
        args.Name.Should().Be(name);
    }

    [TestMethod]
    public void ToArgs_WithInvalidTypeId_ThrowsFormatException()
    {
        var dto = new CreateVariableDtoIn
        {
            TypeId = "not-a-guid",
            Name = "Whatever"
        };

        Action act = () => dto.ToArgs();

        act.Should().Throw<FormatException>();
    }

    [TestMethod]
    public void ToArgs_WithEmptyTypeId_ThrowsFormatException()
    {
        var dto = new CreateVariableDtoIn
        {
            TypeId = string.Empty,
            Name = "Whatever"
        };

        Action act = () => dto.ToArgs();

        act.Should().Throw<FormatException>();
    }
}

