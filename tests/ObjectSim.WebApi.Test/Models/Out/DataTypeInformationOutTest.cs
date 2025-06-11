using FluentAssertions;
using ObjectSim.WebApi.DTOs.Out;
using ValueType = ObjectSim.Domain.ValueType;

namespace ObjectSim.WebApi.Test.Models.Out;

[TestClass]
public class DataTypeInformationDtoOutTest
{
    [TestMethod]
    public void DataTypeToInfo_WithArguments_MapsPropertiesCorrectly()
    {
        var id = Guid.NewGuid();
        var domainDataType = new ValueType(id, "int");

        var result = DataTypeInformationDtoOut.ToInfo(domainDataType);

        result.Should().NotBeNull();
        result.Id.Should().Be(id);
        result.Type.Should().Be("int");
    }

    [TestMethod]
    public void DataTypeToInfo_Properties_AreInitializedCorrectly()
    {
        var id = Guid.NewGuid();
        var dto = new DataTypeInformationDtoOut { Id = id, Type = "string" };

        dto.Id.Should().Be(id);
        dto.Type.Should().Be("string");
    }
}
