using FluentAssertions;
using ObjectSim.WebApi.DTOs.In;

namespace ObjectSim.WebApi.Test.Models.In;

[TestClass]
public class CreateMethodDtoInTest
{
    private CreateMethodDtoIn _dto = null!;

    [TestInitialize]
    public void Setup()
    {
        _dto = new CreateMethodDtoIn
        {
            Name = "Test",
            Type = Guid.NewGuid().ToString(),
            Accessibility = "public",
            IsAbstract = true,
            IsSealed = false,
            IsOverride = true,
            IsVirtual = false,
            IsStatic = false,
            ClassId = Guid.NewGuid().ToString(),
            LocalVariables = [],
            Parameters = [],
            InvokeMethods = []
        };
    }

    #region ToArgs

    [TestMethod]
    public void ToArgs_ShouldReturnExpectedCreateMethodArgs()
    {
        var classId = Guid.NewGuid();
        var typeId = Guid.NewGuid();
        var localVars = new List<CreateVariableDtoIn> { new() { Name = "var1" } };
        var parameters = new List<CreateVariableDtoIn> { new() { Name = "param1" } };
        var invokeIds = new List<CreateInvokeMethodDtoIn> { new() { InvokeMethodId = Guid.NewGuid().ToString(), Reference = "test"} };

        var dto = new CreateMethodDtoIn
        {
            Name = "Test",
            Type = typeId.ToString(),
            Accessibility = "public",
            IsAbstract = true,
            IsSealed = false,
            IsOverride = true,
            IsVirtual = false,
            IsStatic = false,
            ClassId = classId.ToString(),
            LocalVariables = localVars,
            Parameters = parameters,
            InvokeMethods = invokeIds
        };

        var args = dto.ToArgs();

        args.Name.Should().Be("Test");
        args.TypeId.Should().Be(typeId);
        args.Accessibility.Should().Be("public");
        args.IsAbstract.Should().BeTrue();
        args.IsSealed.Should().BeFalse();
        args.IsOverride.Should().BeTrue();
        args.IsVirtual.Should().BeFalse();
        args.IsStatic.Should().BeFalse();
        args.ClassId.Should().Be(classId);
        args.LocalVariables.Should().HaveCount(1);
        args.Parameters.Should().HaveCount(1);
        args.InvokeMethods.Select(x => x.InvokeMethodId.ToString()).Should().BeEquivalentTo(invokeIds.Select(x => x.InvokeMethodId));
    }

    #endregion
}
