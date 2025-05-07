using FluentAssertions;
using Moq;
using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain.Args;
using ObjectSim.Domain;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.BusinessLogic.Test;

[TestClass]
public class MethodSimulatorServiceTest
{
    private Mock<IRepository<Method>> _methodRepositoryMock = null!;
    private Mock<IRepository<DataType>> _dataTypeRepositoryMock = null!;
    private Mock<IRepository<Class>> _classRepositoryMock = null!;
    private IMethodSimulatorService _methodSimulatorServiceTest = null!;

    private SimulateExecutionArgs _simulateArgs = new SimulateExecutionArgs()
    {
        ReferenceType = "UnknownType", InstanceType = "DoesNotMatter", MethodId = Guid.NewGuid(),
    };

    [TestInitialize]
    public void Setup()
    {
        _methodRepositoryMock = new Mock<IRepository<Method>>(MockBehavior.Strict);
        _dataTypeRepositoryMock = new Mock<IRepository<DataType>>(MockBehavior.Strict);
        _classRepositoryMock = new Mock<IRepository<Class>>(MockBehavior.Strict);
        _methodSimulatorServiceTest = new MethodSimulatorService(_dataTypeRepositoryMock.Object, _methodRepositoryMock.Object, _classRepositoryMock.Object);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _methodRepositoryMock.VerifyAll();
        _dataTypeRepositoryMock.VerifyAll();
        _classRepositoryMock.VerifyAll();
    }

    #region Simulate

    #region Error

    [TestMethod]
    public void Simulate_WhenReferenceTypeNotFound_ThrowsException()
    {
        _dataTypeRepositoryMock.Setup(r => r.Get(It.IsAny<Func<DataType, bool>>()))
                           .Returns((DataType)null!);

        Action act = () => _methodSimulatorServiceTest.Simulate(_simulateArgs);

        act.Should().Throw<Exception>().WithMessage("Type 'UnknownType' not found");
    }

    [TestMethod]
    public void Simulate_WhenInstanceTypeNotFound_ThrowsException()
    {
        var referenceType = new ReferenceType("Reference", "ReferenceClass", []);

        _dataTypeRepositoryMock.SetupSequence(r => r.Get(It.IsAny<Func<DataType, bool>>()))
            .Returns(referenceType)
            .Returns((DataType)null!);

        Action act = () => _methodSimulatorServiceTest.Simulate(_simulateArgs);

        act.Should().Throw<Exception>().WithMessage("Type '" + _simulateArgs.InstanceType + "' not found");
    }

    [TestMethod]
    public void Simulate_WhenHierarchyIsInvalid_ThrowsException()
    {
        var args = new SimulateExecutionArgs
        {
            ReferenceType = "VehicleTest", //Vehiculo
            InstanceType = "NotVehicle", //Algo q no es un vehiculo
            MethodId = Guid.NewGuid(), //iniciarViaje
        };

        var methodInNotVehicle = new Method
        {
            Id = Guid.NewGuid(),
            Name = "methodInNotVehicle",
            MethodsInvoke = []
        };

        var referenceType = new ReferenceType("VehicleTest", "VehicleTest", []);
        var instanceType = new ReferenceType("NotVehicle", "NotVehicle", [methodInNotVehicle.Id]);

        _dataTypeRepositoryMock.SetupSequence(r => r.Get(It.IsAny<Func<DataType, bool>>()))
            .Returns(referenceType)
            .Returns(instanceType);

        var parentClass = new Class { Name = "OtherType" };
        var classObj = new Class { Name = "NotVehicle", Parent = parentClass };
        _classRepositoryMock.Setup(r => r.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(classObj);

        Action act = () => _methodSimulatorServiceTest.Simulate(args);

        act.Should().Throw<Exception>().WithMessage($"Parent class 'OtherType' not found in reference type 'VehicleTest'");
    }

    [TestMethod]
    public void Simulate_WhenInstanceIsNotReferenceParent_ThrowsException()
    {
        var args = new SimulateExecutionArgs
        {
            ReferenceType = "VehicleTest", //Vehiculo
            InstanceType = "NotVehicle", //Algo q no es un vehiculo
            MethodId = Guid.NewGuid(), //iniciarViaje
        };

        var methodInNotVehicle = new Method
        {
            Id = Guid.NewGuid(),
            Name = "methodInNotVehicle",
            MethodsInvoke = []
        };

        var referenceType = new ReferenceType("VehicleTest", "VehicleTest", []);
        var instanceType = new ReferenceType("NotVehicle", "NotVehicle", [methodInNotVehicle.Id]);

        _dataTypeRepositoryMock.SetupSequence(r => r.Get(It.IsAny<Func<DataType, bool>>()))
            .Returns(referenceType)
            .Returns(instanceType);

        var classObj = new Class { Name = "NotVehicle", Parent = null };
        _classRepositoryMock.Setup(r => r.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(classObj);

        Action act = () => _methodSimulatorServiceTest.Simulate(args);

        act.Should().Throw<Exception>().WithMessage($"Parent class 'NotVehicle' not found in reference type 'VehicleTest'");
    }

    [TestMethod]
    public void Simulate_WhenMethodDoesNotExistInType_ThrowsException()
    {
        var args = new SimulateExecutionArgs
        {
            ReferenceType = "ReferenceType",
            InstanceType = "ReferenceType",
            MethodId = Guid.NewGuid(),
        };

        var methodId = Guid.NewGuid();
        var referenceType = new ReferenceType("ReferenceType", "ReferenceType", [methodId]);
        var method = new Method
        {
            Id = methodId,
            Name = "MethodName",
            MethodsInvoke = []
        };

        _dataTypeRepositoryMock.SetupSequence(r => r.Get(It.IsAny<Func<DataType, bool>>()))
            .Returns(referenceType)
            .Returns(referenceType);

        var classObj = new Class() {
            Name = "ReferenceType",
            Methods = []
        };

        _classRepositoryMock.Setup(r => r.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(classObj);

        _methodRepositoryMock.Setup(r => r.Get(It.IsAny<Func<Method, bool>>()))
            .Returns(method);

        Action act = () => _methodSimulatorServiceTest.Simulate(args);

        act.Should().Throw<Exception>().WithMessage("Method not found in reference type");
    }

    [TestMethod]
    public void Simulate_WhenMethodDoesNotExist_ThrowsException()
    {
        var methodId = Guid.NewGuid();
        var args = new SimulateExecutionArgs
        {
            ReferenceType = "ReferenceType",
            InstanceType = "ReferenceType",
            MethodId = methodId
        };

        var referenceType = new ReferenceType("ReferenceType", "ReferenceType", []);

        _dataTypeRepositoryMock.SetupSequence(r => r.Get(It.IsAny<Func<DataType, bool>>()))
            .Returns(referenceType)
            .Returns(referenceType);

        _methodRepositoryMock.Setup(r => r.Get(It.IsAny<Func<Method, bool>>()))
            .Returns((Method)null!);

        Action act = () => _methodSimulatorServiceTest.Simulate(args);

        act.Should().Throw<Exception>().WithMessage("Method not found");
    }

    #endregion

    #region Success

    [TestMethod]
    public void Simulate_WhenArgumentsAreValid_Executes()
    {
        var methodId = Guid.NewGuid();
        var invokeMethodId1 = Guid.NewGuid();
        var invokeMethodId2 = Guid.NewGuid();

        var args = new SimulateExecutionArgs
        {
            ReferenceType = "ReferenceType", InstanceType = "ReferenceType", MethodId = methodId
        };

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

        var invokedMethod1 = new Method { Id = invokeMethodId1, Name = "FirstInvoked", MethodsInvoke = [] };
        var invokedMethod2 = new Method { Id = invokeMethodId2, Name = "SecondInvoked", MethodsInvoke = [] };

        var referenceType = new ReferenceType("ReferenceType", "ReferenceType", [methodId]);

        _dataTypeRepositoryMock.SetupSequence(r => r.Get(It.IsAny<Func<DataType, bool>>()))
            .Returns(referenceType)
            .Returns(referenceType);

        var classObj = new Class { Name = "ReferenceType", Methods = new List<Method> { method } };

        _classRepositoryMock.Setup(r => r.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(classObj);

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

        var args = new SimulateExecutionArgs
        {
            ReferenceType = "ReferenceType", InstanceType = "ReferenceType", MethodId = methodId
        };

        var method = new Method
        {
            Id = methodId,
            Name = "MainMethod",
            MethodsInvoke =
            [
                new InvokeMethod(methodId, invokeMethodId1, "this")
            ]
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

        var referenceType = new ReferenceType("ReferenceType", "ReferenceType", [methodId]);

        _dataTypeRepositoryMock.SetupSequence(r => r.Get(It.IsAny<Func<DataType, bool>>()))
            .Returns(referenceType)
            .Returns(referenceType);

        var classObj = new Class { Name = "ReferenceType", Methods = new List<Method> { method } };

        _classRepositoryMock.Setup(r => r.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(classObj);

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
