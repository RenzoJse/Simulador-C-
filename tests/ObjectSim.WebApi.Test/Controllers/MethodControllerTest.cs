using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;
using ObjectSim.WebApi.Controllers;
using ObjectSim.WebApi.DTOs.In;
using ObjectSim.WebApi.DTOs.Out;

namespace ObjectSim.WebApi.Test.Controllers;

[TestClass]
public class MethodControllerTest
{
    private Mock<IMethodService> _methodServiceMock = null!;
    private MethodController _methodController = null!;

    private static readonly DataType TestLocalVariable = new ReferenceType("TestLocalVariable", "string", []);

    private static readonly DataType TestParameter = new ReferenceType("TestParameter", "string", []);

    private static readonly InvokeMethod TestInvokeMethod = new InvokeMethod(Guid.NewGuid(), Guid.NewGuid(), "this");

    private readonly Method _testMethod = new Method
    {
        Name = "TestMethod",
        Type = new ReferenceType("TestParameter", "string", []),
        Accessibility = Method.MethodAccessibility.Public,
        Abstract = false,
        IsOverride = false,
        IsSealed = false,
        Parameters = [TestParameter],
        LocalVariables = [TestLocalVariable],
        MethodsInvoke = [TestInvokeMethod]
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

    #region Create-Method-Test

    [TestMethod]
    public void CreateMethod_WhenIsValid_MakesValidPost()
    {
        _methodServiceMock
             .Setup(service => service.CreateMethod(It.IsAny<CreateMethodArgs>()))
             .Returns(_testMethod);

        var result = _methodController.CreateMethod(new MethodDtoIn
        {
            Name = "TestClass",
            Type = new CreateDataTypeDtoIn(),
            Accessibility = nameof(Method.MethodAccessibility.Public),
            IsAbstract = false,
            IsOverride = false,
            IsSealed = false,
            LocalVariables = [],
            Parameters = [],
            InvokeMethodsId = [],
            ClassId = Guid.NewGuid().ToString()
        });

        var resultObject = result as OkObjectResult;
        var statusCode = resultObject?.StatusCode;
        statusCode.Should().Be(200);

        var answer = resultObject?.Value as MethodInformationDtoOut;
        answer.Should().NotBeNull();
        answer!.Name.Should().Be(_testMethod.Name);
        answer.IsAbstract.Should().Be(_testMethod.Abstract);
        answer.IsOverride.Should().Be(_testMethod.IsOverride);
        answer.IsSealed.Should().Be(_testMethod.IsSealed);
        answer.LocalVariables.Select(lv => lv.Name)
            .Should().BeEquivalentTo(_testMethod.LocalVariables!.Select(lv => lv.Name));
        answer.Parameters.Select(p => p.Name)
            .Should().BeEquivalentTo(_testMethod.Parameters!.Select(p => p.Name));
        answer.InvokeMethodsIds.Should()
            .BeEquivalentTo(_testMethod.MethodsInvoke!.Select(m => m.InvokeMethodId.ToString()));
    }

    #endregion

    #region Delete-Method-Test

    [TestMethod]
    public void DeleteMethod_WhenMethodExists_ShouldReturnOk()
    {
        var methodId = Guid.NewGuid();
        _methodServiceMock
            .Setup(service => service.Delete(methodId))
            .Returns(true);

        var result = _methodController.DeleteMethod(methodId);

        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);
        okResult.Value.Should().Be($"Method with id {methodId} deleted successfully.");
    }

    [TestMethod]
    public void DeleteMethod_WhenMethodDoesNotExist_ShouldThrowKeyNotFoundException()
    {
        var methodId = Guid.NewGuid();

        _methodServiceMock
            .Setup(service => service.Delete(methodId))
            .Throws(new KeyNotFoundException($"Method with id {methodId} not found."));

        Action act = () => _methodController.DeleteMethod(methodId);

        act.Should().Throw<KeyNotFoundException>()
           .WithMessage($"Method with id {methodId} not found.");
    }
    #endregion

    #region GetById-Method-Test
    [TestMethod]
    public void GetMethodById_WhenMethodExists_ShouldReturnOk()
    {
        var methodId = Guid.NewGuid();
        var expectedMethod = _testMethod;

        _methodServiceMock
            .Setup(service => service.GetById(methodId))
            .Returns(expectedMethod);

        var result = _methodController.GetMethodById(methodId);

        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);

        var response = okResult.Value as MethodInformationDtoOut;
        response.Should().NotBeNull();
        response!.Name.Should().Be(expectedMethod.Name);
    }

    [TestMethod]
    [ExpectedException(typeof(KeyNotFoundException))]
    public void GetMethodById_WhenMethodDoesNotExist_ShouldReturnNotFound()
    {
        var methodId = Guid.NewGuid();

        _methodServiceMock
            .Setup(service => service.GetById(methodId))
            .Throws(new KeyNotFoundException("Method with ID not found"));

        var result = _methodController.GetMethodById(methodId);

        var notFoundResult = result as NotFoundObjectResult;
        notFoundResult.Should().NotBeNull();
        notFoundResult!.StatusCode.Should().Be(404);
    }
    #endregion

    #region GetAll-InvokeMethods-Test
    [TestMethod]
    public void GetAllMethods_ShouldReturnAllMethods()
    {
        var methods = new List<Method> { _testMethod };

        _methodServiceMock
            .Setup(service => service.GetAll())
            .Returns(methods);

        var result = _methodController.GetAllMethods();

        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);

        var response = okResult.Value as List<MethodInformationDtoOut>;
        response.Should().NotBeNull();
        response!.Count.Should().Be(methods.Count);
        response.First().Name.Should().Be(_testMethod.Name);
    }
    #endregion

    #region AddInvokeMethod-Test

    [TestMethod]
    public void AddInvokeMethod_WhenEverythingIsValid_ShouldReturnOk()
    {
        var methodId = Guid.NewGuid();
        var invokeMethodId = Guid.NewGuid();
        var reference = "init";
        var invokeMethodDto = new CreateInvokeMethodDtoIn()
        {
            MethodId = methodId,
            InvokeMethodId = invokeMethodId,
            Reference = reference
        };

        _methodServiceMock
            .Setup(service => service.AddInvokeMethod(methodId, [invokeMethodDto]))
            .Returns(_testMethod);

        var result = _methodController.AddInvokeMethod(methodId, new CreateInvokeMethodDtoIn
        {
            MethodId = methodId,
            InvokeMethodId = invokeMethodId,
            Reference = reference
        });

        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);

        var response = okResult.Value as MethodInformationDtoOut;
        response.Should().NotBeNull();
        response!.Name.Should().Be(_testMethod.Name);
    }

    #endregion

}
