using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Moq;
using ObjectSim.Domain;
using ObjectSim.IBusinessLogic;
using ObjectSim.WebApi.Controllers;

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
        _methodController = new MethodController(_methodController.Object);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _methodServiceMock.VerifyAll();
    }
}
