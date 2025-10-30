using FluentAssertions;
using ObjectSim.Domain;
using ObjectSim.WebApi.DTOs.Out;

namespace ObjectSim.WebApi.Test.Models.Out;

[TestClass]
public class CreateVariableDtoOutTest
{
    [TestMethod]
    public void ToInfo_ShouldMapAllPropertiesCorrectly()
    {
        // Arrange
        var method = new Method();
        var variableId = Guid.NewGuid();
        var variable = new Variable(variableId, "myVarName", method);

        var dto = VariableInformatioDtoOut.ToInfo(variable);

        dto.VariableId.Should().Be(variable.VariableId);
        dto.TypeId.Should().Be(variable.TypeId);
        dto.Name.Should().Be(variable.Name);
    }

    [TestMethod]
    public void ToInfo_NullInput_ShouldThrowNullReferenceException()
    {
        Action act = () => VariableInformatioDtoOut.ToInfo(null!);

        act.Should().Throw<NullReferenceException>();
    }
}
