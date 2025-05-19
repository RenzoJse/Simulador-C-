using FluentAssertions;
using Moq;
using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.BusinessLogic.Test;

[TestClass]
public class MethodSimulatorServiceTest
{
    private Mock<IRepository<Method>> _methodRepositoryMock = null!;
    private Mock<IRepository<Class>> _classRepositoryMock = null!;
    private IMethodSimulatorService _methodSimulatorServiceTest = null!;

    [TestInitialize]
    public void Setup()
    {
        _methodRepositoryMock = new Mock<IRepository<Method>>(MockBehavior.Strict);
        _classRepositoryMock = new Mock<IRepository<Class>>(MockBehavior.Strict);
        _methodSimulatorServiceTest = new MethodSimulatorService(_methodRepositoryMock.Object, _classRepositoryMock.Object);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _methodRepositoryMock.VerifyAll();
        _classRepositoryMock.VerifyAll();
    }

    #region Simulate

    #region Error

    [TestMethod]
    public void Simulate_WhenReferenceIdNotFound_ThrowsException()
    {
        var args = new SimulateExecutionArgs
        {
            ReferenceId = Guid.NewGuid(),
            InstanceId = Guid.NewGuid(),
            MethodId = Guid.NewGuid(),
        };

        _classRepositoryMock.Setup(r => r.Get(It.IsAny<Func<Class, bool>>()))
            .Returns((Class)null!);

        Action act = () => _methodSimulatorServiceTest.Simulate(args);

        act.Should().Throw<Exception>().WithMessage("Class not found.");
    }

    [TestMethod]
    public void Simulate_WhenInstanceIdNotFound_ThrowsException()
    {
        var args = new SimulateExecutionArgs
        {
            ReferenceId = Guid.NewGuid(),
            InstanceId = Guid.NewGuid(),
            MethodId = Guid.NewGuid(),
        };

        var referenceClass = new Class { Id = args.ReferenceId, Name = "ReferenceType" };
        _classRepositoryMock.SetupSequence(r => r.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(referenceClass)
            .Returns((Class)null!);

        Action act = () => _methodSimulatorServiceTest.Simulate(args);

        act.Should().Throw<Exception>().WithMessage("Class not found.");
    }

    [TestMethod]
    public void Simulate_WhenInstanceIsNotReferenceParent_ThrowsException()
    {
        var args = new SimulateExecutionArgs
        {
            ReferenceId = Guid.NewGuid(),
            InstanceId = Guid.NewGuid(),
            MethodId = Guid.NewGuid(),
        };

        var classObj = new Class { Name = "InstanceType" };
        _classRepositoryMock.Setup(r => r.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(classObj);

        Action act = () => _methodSimulatorServiceTest.Simulate(args);

        act.Should().Throw<Exception>().WithMessage("Invalid instance type.");
    }

    [TestMethod]
    public void Simulate_WhenHierarchyIsInvalid_ThrowsException()
    {
        var reference = new Class { Id = Guid.NewGuid(), Name = "Vehiculo" };
        var instance = new Class { Id = Guid.NewGuid(), Name = "Auto" };
        var method = new Method { Id = Guid.NewGuid(), Name = "IniciarViaje" };

        _classRepositoryMock.SetupSequence(r => r.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(reference)
            .Returns(instance);

        var args = new SimulateExecutionArgs
        {
            ReferenceId = reference.Id,
            InstanceId = instance.Id,
            MethodId = method.Id
        };

        Action act = () => _methodSimulatorServiceTest.Simulate(args);

        act.Should().Throw<Exception>().WithMessage("Invalid instance type.");
    }

    [TestMethod]
    public void Simulate_WhenMethodDoesNotExistInType_ThrowsException()
    {
        var reference = new Class { Name = "Vehiculo", Methods = [] };
        var instance = new Class { Name = "Auto", Parent = reference, Methods = [] };
        var method = new Method { Id = Guid.NewGuid(), Name = "IniciarViaje" };

        _classRepositoryMock.SetupSequence(r => r.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(reference)
            .Returns(instance);
        _methodRepositoryMock.Setup(r => r.Get(It.IsAny<Func<Method, bool>>()))
            .Returns(method);

        var args = new SimulateExecutionArgs
        {
            ReferenceId = reference.Id,
            InstanceId = instance.Id,
            MethodId = Guid.NewGuid()
        };

        Action act = () => _methodSimulatorServiceTest.Simulate(args);


        act.Should().Throw<Exception>().WithMessage("Method not found in this classes.");
    }

    [TestMethod]
    public void Simulate_WhenMethodDoesNotExist_ThrowsException()
    {
        var reference = new Class { Name = "Vehiculo", Methods = [] };
        var instance = new Class { Name = "Auto", Parent = reference, Methods = [] };

        _classRepositoryMock.SetupSequence(r => r.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(reference)
            .Returns(instance);
        _methodRepositoryMock.Setup(r => r.Get(It.IsAny<Func<Method, bool>>()))
            .Returns((Method)null!);

        var args = new SimulateExecutionArgs
        {
            ReferenceId = reference.Id,
            InstanceId = instance.Id,
            MethodId = Guid.NewGuid()
        };

        Action act = () => _methodSimulatorServiceTest.Simulate(args);

        act.Should().Throw<Exception>();
    }

    #endregion

    #region Success

    [TestMethod]
    public void Simulate_WhenArgumentsAreValid_Executes()
    {
        var methodId = Guid.NewGuid();
        var invokeMethodId1 = Guid.NewGuid();
        var invokeMethodId2 = Guid.NewGuid();

        var method = new Method
        {
            Id = methodId,
            Name = "MainMethod",
            MethodsInvoke =
            [
                new InvokeMethod(methodId, invokeMethodId1, "this"),
                new InvokeMethod(methodId, invokeMethodId2, "other"),
            ]
        };

        var reference = new Class { Name = "Vehiculo", Methods = [] };
        var instance = new Class { Name = "Auto", Parent = reference, Methods = [method] };

        var args = new SimulateExecutionArgs
        {
            ReferenceId = reference.Id,
            InstanceId = instance.Id,
            MethodId = methodId
        };

        var invokedMethod1 = new Method { Id = invokeMethodId1, Name = "FirstInvoked", MethodsInvoke = [] };
        var invokedMethod2 = new Method { Id = invokeMethodId2, Name = "SecondInvoked", MethodsInvoke = [] };

        _classRepositoryMock.SetupSequence(r => r.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(reference)
            .Returns(instance);

        _methodRepositoryMock.SetupSequence(r => r.Get(It.IsAny<Func<Method, bool>>()))
            .Returns(method)
            .Returns(invokedMethod1)
            .Returns(invokedMethod2);

        var result = _methodSimulatorServiceTest.Simulate(args);

        result.Should().Contain("this.FirstInvoked() ->");
        result.Should().Contain("other.SecondInvoked() ->");
    }

    [TestMethod]
    public void Simulate_WhenArgumentsAreValidAndInvokedMethodInvokeOtherMethods_Executes()
    {
        var methodId = Guid.NewGuid();
        var invokeMethodId1 = Guid.NewGuid();
        var invokeMethodId2 = Guid.NewGuid();

        var method = new Method
        {
            Id = methodId,
            Name = "MainMethod",
            MethodsInvoke =
            [
                new InvokeMethod(methodId, invokeMethodId1, "this")
            ]
        };

        var reference = new Class { Name = "Vehiculo", Methods = [] };
        var instance = new Class { Name = "Auto", Parent = reference, Methods = [method] };

        var args = new SimulateExecutionArgs
        {
            ReferenceId = reference.Id,
            InstanceId = instance.Id,
            MethodId = methodId
        };

        var invokedMethod1 = new Method
        {
            Id = invokeMethodId1,
            Name = "FirstInvoked",
            MethodsInvoke =
            [
                new InvokeMethod(methodId, invokeMethodId2, "this")
            ]
        };

        var invokedMethod2 = new Method { Id = invokeMethodId2, Name = "SecondInvoked" };

        _classRepositoryMock.SetupSequence(r => r.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(reference)
            .Returns(instance);

        _methodRepositoryMock.SetupSequence(r => r.Get(It.IsAny<Func<Method, bool>>()))
            .Returns(method)
            .Returns(invokedMethod1)
            .Returns(invokedMethod2);

        var result = _methodSimulatorServiceTest.Simulate(args);
        result.Should().Contain("this.FirstInvoked() ->");
        result.Should().Contain("     this.SecondInvoked() ->");
    }

    #endregion

    #endregion
}
