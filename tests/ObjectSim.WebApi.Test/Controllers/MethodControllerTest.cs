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

    #region CreateAttribute-Method-Test
    [TestMethod]
    public void CreateMethod_WhenIsValid_MakesValidPost()
    {
        _methodServiceMock
             .Setup(service => service.CreateMethod(It.IsAny<CreateMethodArgs>()))
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
            InvokeMethodsId = [],
            ClassId = Guid.NewGuid().ToString()
        });

        var resultObject = result as OkObjectResult;
        var statusCode = resultObject?.StatusCode;
        statusCode.Should().Be(200);

        var answer = resultObject?.Value as MethodOutModel;
        answer.Should().NotBeNull();
        answer!.Name.Should().Be(_testMethod.Name);
        answer.IsAbstract.Should().Be(_testMethod.Abstract);
        answer.IsOverride.Should().Be(_testMethod.IsOverride);
        answer.IsSealed.Should().Be(_testMethod.IsSealed);
        answer.LocalVariables.Select(lv => lv.Name)
            .Should().BeEquivalentTo(_testMethod.LocalVariables!.Select(lv => lv.Name));
        answer.Parameters.Select(p => p.Name)
            .Should().BeEquivalentTo(_testMethod.Parameters!.Select(p => p.Name));
        answer.Methods.Select(m => m.Name)
            .Should().BeEquivalentTo(_testMethod.MethodsInvoke!.Select(m => m.Name));
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
    public void DeleteMethod_WhenMethodDoesNotExist_ShouldReturnNotFound()
    {
        var methodId = Guid.NewGuid();
        _methodServiceMock
            .Setup(service => service.Delete(methodId))
            .Returns(false);

        var result = _methodController.DeleteMethod(methodId);

        var notFoundResult = result as NotFoundObjectResult;
        notFoundResult.Should().NotBeNull();
        notFoundResult!.StatusCode.Should().Be(404);
        notFoundResult.Value.Should().Be($"Method with id {methodId} not found.");
    }
    #endregion
    /*
        #region Update-Method-Test
        [TestMethod]
        public void UpdateMethod_WhenMethodExists_ShouldReturnOk()
        {
            var methodId = Guid.NewGuid();
            var updatedMethod = new Method
            {
                Name = "UpdatedMethod",
                Type = Method.MethodDataType.Char,
                Accessibility = Method.MethodAccessibility.Internal,
                Abstract = false,
                IsSealed = false,
                IsOverride = false,
                LocalVariables = [],
                Parameters = [],
                MethodsInvoke = []
            };

            _methodServiceMock
                .Setup(service => service.Update(methodId, It.IsAny<Method>()))
                .Returns(updatedMethod);

            var updateDto = new MethodDtoIn
            {
                Name = "UpdatedMethod",
                Type = Method.MethodDataType.Char.ToString(),
                Accessibility = Method.MethodAccessibility.Internal.ToString(),
                IsAbstract = false,
                IsOverride = false,
                IsSealed = false,
                LocalVariables = [],
                Parameters = [],
                InvokeMethodsId = []
            };

            var result = _methodController.UpdateMethod(methodId, updateDto);

            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);

            var response = okResult.Value as MethodOutModel;
            response.Should().NotBeNull();
            response!.Name.Should().Be(updatedMethod.Name);
        }

        [TestMethod]
        public void UpdateMethod_WhenMethodDoesNotExist_ShouldReturnNotFound()
        {
            var methodId = Guid.NewGuid();

            _methodServiceMock
                .Setup(service => service.Update(methodId, It.IsAny<Method>()))
                .Returns((Method?)null);

            var updateDto = new MethodDtoIn
            {
                Name = "UpdatedMethod",
                Type = Method.MethodDataType.Char.ToString(),
                Accessibility = Method.MethodAccessibility.Internal.ToString(),
                IsAbstract = false,
                IsOverride = false,
                IsSealed = false,
                LocalVariables = [],
                Parameters = [],
                InvokeMethodsId = []
            };

            var result = _methodController.UpdateMethod(methodId, updateDto);

            var notFoundResult = result as NotFoundObjectResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult!.StatusCode.Should().Be(404);
            notFoundResult.Value.Should().Be($"Method with id {methodId} not found.");
        }
        #endregion
    */
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

        var response = okResult.Value as MethodOutModel;
        response.Should().NotBeNull();
        response!.Name.Should().Be(expectedMethod.Name);
    }

    [TestMethod]
    public void GetMethodById_WhenMethodDoesNotExist_ShouldReturnNotFound()
    {
        var methodId = Guid.NewGuid();

        _methodServiceMock
            .Setup(service => service.GetById(methodId))
            .Returns((Method?)null);

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

        var response = okResult.Value as List<MethodOutModel>;
        response.Should().NotBeNull();
        response!.Count.Should().Be(methods.Count);
        response.First().Name.Should().Be(_testMethod.Name);
    }
    #endregion

    #region Add-LocalVariable-Test
    [TestMethod]
    public void AddLocalVariable_WhenValid_ShouldReturnOk()
    {
        var methodId = Guid.NewGuid();
        var dto = new LocalVariableDtoIn
        {
            Name = "lvTest",
            Type = "Int"
        };

        var localVarEntity = dto.ToEntity();

        _methodServiceMock
            .Setup(s => s.AddLocalVariable(methodId, It.IsAny<LocalVariable>()))
            .Returns(localVarEntity);

        var result = _methodController.AddLocalVariable(methodId, dto);

        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);

        var returned = okResult.Value as LocalVariableOutModel;
        returned.Should().NotBeNull();
        returned!.Name.Should().Be("lvTest");
        returned.Type.Should().Be("Int");
    }

    [TestMethod]
    public void AddLocalVariable_WhenMethodNotFound_ShouldReturnBadRequest()
    {
        var methodId = Guid.NewGuid();
        var dto = new LocalVariableDtoIn
        {
            Name = "lvTest",
            Type = "Bool"
        };

        _methodServiceMock
            .Setup(s => s.AddLocalVariable(methodId, It.IsAny<LocalVariable>()))
            .Throws(new Exception("Method not found"));

        var result = _methodController.AddLocalVariable(methodId, dto);

        var badRequest = result as BadRequestObjectResult;
        badRequest.Should().NotBeNull();
        badRequest!.StatusCode.Should().Be(400);
        badRequest.Value.Should().Be("Method not found");
    }

    [TestMethod]
    public void AddLocalVariable_WhenDuplicate_ShouldReturnBadRequest()
    {
        var methodId = Guid.NewGuid();
        var dto = new LocalVariableDtoIn
        {
            Name = "lvTest",
            Type = "Int"
        };

        _methodServiceMock
            .Setup(s => s.AddLocalVariable(methodId, It.IsAny<LocalVariable>()))
            .Throws(new Exception("LocalVariable already exists in this method"));

        var result = _methodController.AddLocalVariable(methodId, dto);

        var badRequest = result as BadRequestObjectResult;
        badRequest.Should().NotBeNull();
        badRequest!.StatusCode.Should().Be(400);
        badRequest.Value.Should().Be("LocalVariable already exists in this method");
    }
    #endregion

    #region Add-Parameter-Test
    [TestMethod]
    public void AddParameter_WhenValid_ShouldReturnOk()
    {
        var methodId = Guid.NewGuid();
        var dto = new ParameterDtoIn
        {
            Name = "parameterTest",
            Type = "String"
        };

        var paramEntity = dto.ToEntity();

        _methodServiceMock
            .Setup(s => s.AddParameter(methodId, It.IsAny<Parameter>()))
            .Returns(paramEntity);

        var result = _methodController.AddParameter(methodId, dto);

        var ok = result as OkObjectResult;
        ok.Should().NotBeNull();
        ok!.StatusCode.Should().Be(200);

        var returned = ok.Value as ParameterOutModel;
        returned.Should().NotBeNull();
        returned!.Name.Should().Be("parameterTest");
        returned.Type.Should().Be("String");
    }

    [TestMethod]
    public void AddParameter_WhenMethodNotFound_ShouldReturnBadRequest()
    {
        var methodId = Guid.NewGuid();
        var dto = new ParameterDtoIn
        {
            Name = "parameterTest",
            Type = "Int"
        };

        _methodServiceMock
            .Setup(s => s.AddParameter(methodId, It.IsAny<Parameter>()))
            .Throws(new Exception("Method not found"));

        var result = _methodController.AddParameter(methodId, dto);

        var badRequest = result as BadRequestObjectResult;
        badRequest.Should().NotBeNull();
        badRequest!.StatusCode.Should().Be(400);
        badRequest.Value.Should().Be("Method not found");
    }

    [TestMethod]
    public void AddParameter_WhenDuplicate_ShouldReturnBadRequest()
    {
        var methodId = Guid.NewGuid();
        var dto = new ParameterDtoIn
        {
            Name = "parameterTest",
            Type = "Bool"
        };

        _methodServiceMock
            .Setup(s => s.AddParameter(methodId, It.IsAny<Parameter>()))
            .Throws(new Exception("Parameter already exists in this method"));

        var result = _methodController.AddParameter(methodId, dto);

        var badRequest = result as BadRequestObjectResult;
        badRequest.Should().NotBeNull();
        badRequest!.StatusCode.Should().Be(400);
        badRequest.Value.Should().Be("Parameter already exists in this method");
    }

    #endregion
}
