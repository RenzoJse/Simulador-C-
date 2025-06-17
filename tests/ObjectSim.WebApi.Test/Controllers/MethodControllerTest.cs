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
    private Variable? _testLocalVariable;
    private Variable? _testParameter;
    private Method? _testMethod;

    private static readonly InvokeMethod TestInvokeMethod = new InvokeMethod(Guid.NewGuid(), Guid.NewGuid(), "this");

    private static readonly ReferenceType TestReferenceType = new ReferenceType(Guid.NewGuid(), "string");

    [TestInitialize]
    public void Setup()
    {
        _methodServiceMock = new Mock<IMethodService>();
        _methodController = new MethodController(_methodServiceMock.Object);

        _testMethod = new Method
        {
            Name = "TestMethod",
            TypeId = TestReferenceType.Id,
            Accessibility = Method.MethodAccessibility.Public,
            Abstract = false,
            IsOverride = false,
            IsSealed = false,
            Parameters = [],
            LocalVariables = [],
            MethodsInvoke = [TestInvokeMethod]
        };
        _testLocalVariable = new Variable(Guid.NewGuid(), "string", _testMethod);
        _testParameter = new Variable(Guid.NewGuid(), "int", _testMethod);

        _testMethod.LocalVariables = [_testLocalVariable];
        _testMethod.Parameters = [_testParameter];
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
             .Returns(_testMethod!);

        var result = _methodController.CreateMethod(new CreateMethodDtoIn
        {
            Name = "TestMethod",
            Type = Guid.NewGuid().ToString(),
            Accessibility = nameof(Method.MethodAccessibility.Public),
            IsAbstract = false,
            IsOverride = false,
            IsSealed = false,
            LocalVariables = [],
            Parameters = [],
            InvokeMethods = [],
            ClassId = Guid.NewGuid().ToString()
        });

        var resultObject = result as OkObjectResult;
        var statusCode = resultObject?.StatusCode;
        statusCode.Should().Be(200);

        var answer = resultObject?.Value as MethodInformationDtoOut;
        answer.Should().NotBeNull();
        answer.Name.Should().Be(_testMethod!.Name);
        answer.IsAbstract.Should().Be(_testMethod.Abstract);
        answer.IsOverride.Should().Be(_testMethod.IsOverride);
        answer.IsSealed.Should().Be(_testMethod.IsSealed);
        answer.LocalVariables.Select(lv => lv.Name)
            .Should().BeEquivalentTo(_testMethod.LocalVariables.Select(lv => lv.Name));
        answer.Parameters.Select(p => p.Name)
            .Should().BeEquivalentTo(_testMethod.Parameters!.Select(p => p.Name));
        answer.InvokeMethodsIds.Should()
            .BeEquivalentTo(_testMethod.MethodsInvoke!.Select(m => m.InvokeMethodId.ToString()));
    }

    [TestMethod]
    public void CreateMethod_NullDto_ShouldThrowNullReferenceException()
    {
        Action act = () => _methodController.CreateMethod(null!);
        act.Should().Throw<NullReferenceException>();
    }

    [TestMethod]
    public void CreateMethod_ServiceThrowsException_ShouldPropagateException()
    {
        var dto = new CreateMethodDtoIn
        {
            Name = "TestMethod",
            Type = Guid.NewGuid().ToString(),
            Accessibility = nameof(Method.MethodAccessibility.Public),
            IsAbstract = false,
            IsOverride = false,
            IsSealed = false,
            LocalVariables = [],
            Parameters = [],
            InvokeMethods = [],
            ClassId = Guid.NewGuid().ToString()
        };

        _methodServiceMock
            .Setup(s => s.CreateMethod(It.IsAny<CreateMethodArgs>()))
            .Throws(new InvalidOperationException("internal error"));

        Action act = () => _methodController.CreateMethod(dto);
        act.Should().Throw<InvalidOperationException>()
           .WithMessage("internal error");
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

    [TestMethod]
    public void DeleteMethod_WhenServiceReturnsFalse_ShouldStillReturnOkWithMessage()
    {
        var id = Guid.NewGuid();
        _methodServiceMock
            .Setup(s => s.Delete(id))
            .Returns(false);

        var result = _methodController.DeleteMethod(id) as OkObjectResult;
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(200);
        result.Value.Should().Be($"Method with id {id} deleted successfully.");
    }

    [TestMethod]
    public void DeleteMethod_EmptyGuid_ShouldThrowArgumentException()
    {
        _methodServiceMock
            .Setup(s => s.Delete(Guid.Empty))
            .Throws(new ArgumentException("Id cannot be empty"));

        Action act = () => _methodController.DeleteMethod(Guid.Empty);
        act.Should().Throw<ArgumentException>()
           .WithMessage("Id cannot be empty");
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
            .Returns(expectedMethod!);

        var result = _methodController.GetMethodById(methodId);

        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);

        var response = okResult.Value as MethodInformationDtoOut;
        response.Should().NotBeNull();
        response!.Name.Should().Be(expectedMethod!.Name);
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

    [TestMethod]
    public void GetMethodById_ServiceThrowsInvalidOperationException_ShouldPropagateException()
    {
        var id = Guid.NewGuid();
        _methodServiceMock
            .Setup(s => s.GetById(id))
            .Throws(new InvalidOperationException("Error"));

        Action act = () => _methodController.GetMethodById(id);
        act.Should().Throw<InvalidOperationException>()
           .WithMessage("Error");
    }

    [TestMethod]
    public void GetMethodById_EmptyGuid_ShouldThrowArgumentException()
    {
        _methodServiceMock
            .Setup(s => s.GetById(Guid.Empty))
            .Throws(new ArgumentException("Invalid id"));

        Action act = () => _methodController.GetMethodById(Guid.Empty);
        act.Should().Throw<ArgumentException>()
           .WithMessage("Invalid id");
    }
    #endregion

    #region GetAll-InvokeMethods-Test
    [TestMethod]
    public void GetAllMethods_ShouldReturnAllMethods()
    {
        var methods = new List<Method> { _testMethod! };

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
        response.First().Name.Should().Be(_testMethod!.Name);
    }

    [TestMethod]
    public void GetAllMethods_WhenNoMethodsExist_ShouldReturnEmptyList()
    {
        _methodServiceMock
            .Setup(s => s.GetAll())
            .Returns([]);

        var ok = _methodController.GetAllMethods() as OkObjectResult;
        ok.Should().NotBeNull();
        ok!.StatusCode.Should().Be(200);
        var list = ok.Value as List<MethodInformationDtoOut>;
        list.Should().NotBeNull().And.BeEmpty();
    }

    [TestMethod]
    public void GetAllMethods_ServiceThrowsException_ShouldPropagateException()
    {
        _methodServiceMock
            .Setup(s => s.GetAll())
            .Throws(new Exception("error get all"));

        Action act = () => _methodController.GetAllMethods();
        act.Should().Throw<Exception>()
           .WithMessage("error get all");
    }
    #endregion

    #region AddInvokeMethod-Test
    [TestMethod]
    public void AddInvokeMethod_WhenEverythingIsValid_ShouldReturnOk()
    {
        var methodId = Guid.NewGuid();
        var invokeMethodId = Guid.NewGuid();
        const string reference = "init";

        _methodServiceMock
            .Setup(service => service.AddInvokeMethod(methodId, It.IsAny<List<CreateInvokeMethodArgs>>()))
            .Returns(_testMethod!);

        var result = _methodController.AddInvokeMethods(methodId, [new CreateInvokeMethodDtoIn
        {
            InvokeMethodId = invokeMethodId.ToString(),
            Reference = reference
        }]);

        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);

        var response = okResult.Value as MethodInformationDtoOut;
        response.Should().NotBeNull();
        response!.Name.Should().Be(_testMethod!.Name);
    }

    [TestMethod]
    public void AddInvokeMethods_ServiceThrowsException_ShouldPropagateException()
    {
        var methodId = Guid.NewGuid();
        _methodServiceMock
            .Setup(s => s.AddInvokeMethod(methodId, It.IsAny<List<CreateInvokeMethodArgs>>()))
            .Throws(new InvalidOperationException("error add invoke"));

        Action act = () => _methodController.AddInvokeMethods(methodId, []);
        act.Should().Throw<InvalidOperationException>()
           .WithMessage("error add invoke");
    }

    [TestMethod]
    public void AddInvokeMethods_WithEmptyList_ShouldReturnOkWithMapping()
    {
        var methodId = Guid.NewGuid();
        _methodServiceMock
            .Setup(s => s.AddInvokeMethod(methodId, []))
            .Returns(_testMethod!);

        var result = _methodController.AddInvokeMethods(methodId, []) as OkObjectResult;
        result.Should().NotBeNull();
        var dto = result!.Value as MethodInformationDtoOut;
        dto.Should().NotBeNull();
        dto!.InvokeMethodsIds.Should().BeEquivalentTo(_testMethod!.MethodsInvoke!.Select(x => x.InvokeMethodId.ToString()));
    }

    #endregion

}
