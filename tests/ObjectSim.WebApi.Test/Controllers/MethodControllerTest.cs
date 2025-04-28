using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ObjectSim.Domain;
using ObjectSim.IBusinessLogic;
using ObjectSim.WebApi.Controllers;
using ObjectSim.WebApi.DTOs.In;
using ObjectSim.WebApi.DTOs.Out;

namespace ObjectSim.WebApi.Test.Controllers;
public class MethodControllerTest
{
    private Mock<IMethodService> _methodServiceMock = null!;
    private MethodController _methodController = null!;

    private static readonly LocalVariable TestLocalVariable = new LocalVariable
    {
        Name = "TestLocalVariable",
    };

    private static readonly Parameter TestParameter = new Parameter
    {
        Name = "TestParameter",
    };

    private static readonly Method TestMethod = new Method
    {
        Name = "TestMethod",
    };

    private readonly Method _testMethod = new Method
    {
        Name = "TestMethod",
        Type = Method.MethodDataType.String,
        Accessibility = Method.MethodAccessibility.Public,
        Abstract = false,
        IsOverride = false,
        IsSealed = false,
        Parameters = [TestParameter],
        LocalVariables = [TestLocalVariable],
        MethodsInvoke = [TestMethod]
    };

    [TestInitialize]
    public void Setup()
    {
        _methodServiceMock = new Mock<IMethodService>();
        _methodController = new MethodController(_methodServiceMock.Object);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _methodServiceMock.VerifyAll();
    }

    [TestMethod]
    public void CreateClass_WhenIsValid_MakesValidPost()
    {
        _methodServiceMock
             .Setup(service => service.Create(It.IsAny<Method>()))
             .Returns(_testMethod);

        var result = _methodController.CreateMethod(new MethodDtoIn
        {
            Name = "TestClass",
            Type = Method.MethodDataType.Char.ToString(),
            Accessibility = Method.MethodAccessibility.Public.ToString(),
            IsAbstract = false,
            IsOverride = false,
            IsSealed = false,
            LocalVariables = [],
            Parameters = [],
            Methods = [],
        });

        var resultObject = result as OkObjectResult;
        var statusCode = resultObject?.StatusCode;
        statusCode.Should().Be(200);

        var answer = resultObject?.Value as MethodOutModel;
        answer.Should().NotBeNull();
        answer.Name.Should().Be(_testMethod.Name);
        answer.IsAbstract.Should().Be((bool)_testMethod.Abstract!);
        answer.IsOverride.Should().Be((bool)_testMethod.IsOverride!);
        answer.IsSealed.Should().Be((bool)_testMethod.IsSealed!);
        answer.LocalVariables.Should().BeEquivalentTo(_testMethod.LocalVariables!.Select(localVariable => localVariable.Name));
        answer.Parameters.Should().BeEquivalentTo(_testMethod.Parameters!.Select(parameter => parameter.Name));
        answer.Methods.Should().BeEquivalentTo(_testMethod.MethodsInvoke!.Select(method => method.Name));
    }
}
