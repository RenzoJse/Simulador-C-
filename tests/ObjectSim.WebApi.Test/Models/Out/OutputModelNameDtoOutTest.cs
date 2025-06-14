using ObjectSim.WebApi.DTOs.Out;

namespace ObjectSim.WebApi.Test.Models.Out;

[TestClass]
public class OutputModelNameDtoOutTest
{
    [TestMethod]
    public void Name_ShouldBeEmpty_ByDefault()
    {
        var dto = new OutputModelNameDtoOut();
        Assert.AreEqual(string.Empty, dto.Name);
    }

    [TestMethod]
    public void ToInfo_ShouldSetNameCorrectly()
    {
        const string name = "TestName";
        var dto = OutputModelNameDtoOut.ToInfo(name);
        Assert.AreEqual(name, dto.Name);
    }
}
