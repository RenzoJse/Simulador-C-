using Moq;
using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;

namespace ObjectSim.BusinessLogic.Test;

[TestClass]
public class InvokeMethodServiceTest
{
    private Method _testMethod = new Method
    {
        Id = Guid.NewGuid(),
        Name = "TestMethod",
        MethodsInvoke = []
    };

    private Mock<IRepository<InvokeMethod>>? _invokeMethodRepositoryMock;
    private InvokeMethodService? _invokeMethodServiceTest;

    #region Setup & Cleanup

    [TestInitialize]
    public void Setup()
    {
        _invokeMethodRepositoryMock = new Mock<IRepository<InvokeMethod>>(MockBehavior.Strict);
        _invokeMethodServiceTest = new InvokeMethodService(_invokeMethodRepositoryMock.Object);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _invokeMethodRepository!.VerifyAll();
    }

    #endregion

    #region MyRegion

    #region Error

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CreateInvokeMethod_WithNullMethod_ThrowsArgumentNullException()
    {
        var invokeMethodArgs = new CreateInvokeMethodArgs(Guid.NewGuid(), "reference");

        Method? method = null;

        _invokeMethodServiceTest!.CreateInvokeMethod(invokeMethodArgs, method!);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CreateInvokeMethod_AddNullInvokeMethod_ThrowsArgumentNullException()
    {
        _invokeMethodServiceTest!.CreateInvokeMethod(null, _testMethod!);
    }

#endregion

    #region Success

    [TestMethod]
    public void CreateInvokeMethod_Success()
    {
        var invokeMethodArgs = new CreateInvokeMethodArgs(Guid.NewGuid(), "reference");

        _invokeMethodRepositoryMock!.Setup(repo => repo.Add(It.IsAny<InvokeMethod>()))
            .Returns((InvokeMethod invokeMethod) => invokeMethod);

        var invokeMethod = _invokeMethodServiceTest!.CreateInvokeMethod(invokeMethodArgs, _testMethod!);

        Assert.IsNotNull(invokeMethod);
        Assert.AreEqual(invokeMethodArgs.InvokeMethodId, invokeMethod.InvokeMethodId);
        Assert.AreEqual(invokeMethodArgs.Reference, invokeMethod.Reference);
        Assert.IsTrue(_testMethod.MethodsInvoke.Contains(invokeMethod));
    }

    #endregion

    #endregion
}
