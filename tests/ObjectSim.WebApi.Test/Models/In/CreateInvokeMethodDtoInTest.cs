using FluentAssertions;
using ObjectSim.WebApi.DTOs.In;

namespace ObjectSim.WebApi.Test.Models.In;

[TestClass]
public class CreateInvokeMethodDtoInTest
{

    [TestMethod]
    public void CreateDataTypeDtoIn_WithArguments_ReturnsNewInstance()
    {
        var model = new CreateInvokeMethodDtoIn { InvokeMethodId = Guid.NewGuid().ToString(), Reference = "init" };
        model.InvokeMethodId.Should().NotBe(Guid.Empty.ToString());
        model.Reference.Should().Be("init");
    }

    [TestMethod]
    public void CreateDataTypeDtoInToArgs_WithArguments_DataTypeArgs()
    {
        var model = new CreateInvokeMethodDtoIn { InvokeMethodId = Guid.NewGuid().ToString(), Reference = "init" };
        var args = model.ToArgs();
        args.Should().NotBeNull();
        args.InvokeMethodId.Should().NotBe(Guid.Empty);
        args.Reference.Should().Be("init");
    }

}
