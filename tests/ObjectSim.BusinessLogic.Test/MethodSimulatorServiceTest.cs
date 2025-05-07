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
    }

    #region Simulate

    #region Error

    [TestMethod]
    public void Simulate_WhenReferenceTypeNotFound_ThrowsException()
    {
        var args = new SimulateExecutionArgs
        {
            ReferenceType = "UnknownType",
            InstanceType = "DoesNotMatter",
            MethodId = "AnyMethod"
        };

        _dataTypeRepositoryMock.Setup(r => r.Get(It.IsAny<Func<DataType, bool>>()))
                           .Returns((DataType)null!);

        Action act = () => _methodSimulatorServiceTest.Simulate(args);

        act.Should().Throw<Exception>().WithMessage("Type 'UnknownType' not found");
    }

    [TestMethod]
    public void Simulate_WhenInstanceTypeNotFound_ThrowsException()
    {
        var args = new SimulateExecutionArgs
        {
            ReferenceType = "ReferenceType",
            InstanceType = "UnknownType",
            MethodId = "AnyMethod"
        };

        var referenceType = new ReferenceType("Reference", "ReferenceClass", []);

        _dataTypeRepositoryMock.SetupSequence(r => r.Get(It.IsAny<Func<DataType, bool>>()))
            .Returns(referenceType)
            .Returns((DataType)null!);

        Action act = () => _methodSimulatorServiceTest.Simulate(args);

        act.Should().Throw<Exception>().WithMessage("Type 'UnknownType' not found");
    }

    [TestMethod]
    public void Simulate_WhenHierarchyIsInvalid_ThrowsException()
    {
        var args = new SimulateExecutionArgs
        {
            ReferenceType = "VehicleTest", //Vehiculo
            InstanceType = "NotVehicle", //Algo q no es un vehiculo
            MethodId = "methodInNotVehicle" //iniciarViaje
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
            MethodId = "methodInNotVehicle" //iniciarViaje
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
            MethodId = "NonExistentMethod"
        };

        var methodId = Guid.NewGuid();
        var referenceType = new ReferenceType("ReferenceType", "ReferenceType", [methodId]);

        _dataTypeRepositoryMock.SetupSequence(r => r.Get(It.IsAny<Func<DataType, bool>>()))
            .Returns(referenceType)
            .Returns(referenceType);

        var classObj = new Class() { Name = "ReferenceType" };
        _classRepositoryMock.Setup(r => r.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(classObj);

        _methodRepositoryMock.Setup(r => r.Get(It.IsAny<Func<Method, bool>>()))
            .Returns((Method)null!);

        Action act = () => _methodSimulatorServiceTest.Simulate(args);

        act.Should().Throw<Exception>().WithMessage("Method not found in reference type");
    }

    #endregion

    #endregion

    /*[TestMethod]
    public void Simulate_ShouldReturnCorrectTrace()
    {
        var method2 = new Method { Id = Guid.NewGuid(), Name = "SubStep" };
        var method1 = new Method { Id = Guid.NewGuid(), Name = "MainStep", MethodsInvoke = [] };

        var referenceType = new ReferenceType("Reference", "ReferenceClass", [method1.Id]);
        var instanceType = new ReferenceType("Instance", "ReferenceClass", [method2.Id]);

        var refRepoMock = new Mock<IRepository<DataType>>();
        refRepoMock.SetupSequence(r => r.Get(It.IsAny<Func<DataType, bool>>()))
            .Returns(referenceType)
            .Returns(instanceType);

        var methodRepoMock = new Mock<IRepository<Method>>();
        methodRepoMock.Setup(r => r.Get(It.IsAny<Func<Method, bool>>()))
            .Returns<Func<Method, bool>>(pred => pred(method1) ? method1 : method2);

        var args = new SimulateExecutionArgs
        {
            ReferenceType = "ReferenceClass",
            InstanceType = "ReferenceClass",
            MethodId = "MainStep"
        };

        var service = new MethodSimulatorService(refRepoMock.Object, methodRepoMock.Object);

        var result = service.Simulate(args);

        result.Should().ContainInOrder(
            "Instance.this.MainStep()",
            "Instance.this.SubStep()"
        );
    }

    [TestMethod]
    public void Simulate_ShouldThrowIfTypeNotFound()
    {
        var refRepoMock = new Mock<IRepository<DataType>>();
        refRepoMock.Setup(r => r.Get(It.IsAny<Func<DataType, bool>>()))
                   .Returns((DataType)null!);

        var methodRepoMock = new Mock<IRepository<Method>>();

        var args = new SimulateExecutionArgs
        {
            ReferenceType = "UnknownType",
            InstanceType = "DoesNotMatter",
            MethodId = "AnyMethod"
        };

        var service = new MethodSimulatorService(refRepoMock.Object, methodRepoMock.Object);

        Action act = () => service.Simulate(args);

        act.Should().Throw<Exception>().WithMessage("Type 'UnknownType' not found");
    }*/
}
