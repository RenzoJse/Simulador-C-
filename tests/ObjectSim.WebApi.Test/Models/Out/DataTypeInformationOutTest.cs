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
        var domainDataType = new ValueType("TestName", "int");

        var result = DataTypeInformationDtoOut.ToInfo(domainDataType);

        result.Should().NotBeNull();
        result.Name.Should().Be("TestName");
        result.Type.Should().Be("int");
    }

    [TestMethod]
    public void DataTypeToInfo_Properties_AreInitializedCorrectly()
    {
        var dto = new DataTypeInformationDtoOut { Name = "TestName", Type = "string" };

        dto.Name.Should().Be("TestName");
        dto.Type.Should().Be("string");
    }
}
