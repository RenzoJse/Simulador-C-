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
    private IMethodSimulatorService _methodSimulatorServiceTest = null!;

    [TestInitialize]
    public void Setup()
    {
        _methodRepositoryMock = new Mock<IRepository<Method>>(MockBehavior.Strict);
        _dataTypeRepositoryMock = new Mock<IRepository<DataType>>(MockBehavior.Strict);
        _methodSimulatorServiceTest = new MethodSimulatorService(_dataTypeRepositoryMock.Object, _methodRepositoryMock.Object);
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
            MethodName = "AnyMethod"
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
            MethodName = "AnyMethod"
        };

        var referenceType = new ReferenceType("Reference", "ReferenceClass", []);

        _dataTypeRepositoryMock.SetupSequence(r => r.Get(It.IsAny<Func<DataType, bool>>()))
            .Returns(referenceType)
            .Returns((DataType)null!);

        Action act = () => _methodSimulatorServiceTest.Simulate(args);

        act.Should().Throw<Exception>().WithMessage("Type 'UnknownType' not found");
    }

    #endregion

    #endregion

    [TestMethod]
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
            MethodName = "MainStep"
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
            MethodName = "AnyMethod"
        };

        var service = new MethodSimulatorService(refRepoMock.Object, methodRepoMock.Object);

        Action act = () => service.Simulate(args);

        act.Should().Throw<Exception>().WithMessage("Type 'UnknownType' not found");
    }
}
