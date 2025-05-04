using FluentAssertions;
using Moq;
using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain.Args;
using ObjectSim.Domain;

namespace ObjectSim.BusinessLogic.Test;

[TestClass]
public class MethodSimulatorServiceTest
{
    [TestMethod]
    public void Simulate_ShouldReturnMethodCallChain()
    {
        var reference = new Class { Id = Guid.NewGuid(), Name = "Class1" };
        var instance = new Class { Id = Guid.NewGuid(), Name = "Class2" };

        var method2 = new Method
        {
            Id = Guid.NewGuid(),
            Name = "Method2",
            ClassId = instance.Id
        };

        var method1 = new Method
        {
            Id = Guid.NewGuid(),
            Name = "Method1",
            ClassId = reference.Id,
            MethodsInvoke = new List<Method> { method2 }
        };

        var classRepoMock = new Mock<IRepository<Class>>();
        classRepoMock.Setup(r => r.Get(It.IsAny<Func<Class, bool>>()))
            .Returns((Func<Class, bool> predicate) =>
            {
                var allClasses = new List<Class> { reference, instance };
                return allClasses.FirstOrDefault(predicate);
            });

        var methodRepoMock = new Mock<IRepository<Method>>();
        methodRepoMock.Setup(r => r.Get(It.IsAny<Func<Method, bool>>()))
            .Returns(method1);

        var service = new MethodSimulatorService(classRepoMock.Object, methodRepoMock.Object);

        var args = new SimulateExecutionArgs
        {
            ReferenceClassId = reference.Id,
            InstanceClassId = instance.Id,
            MethodToExecuteId = method1.Id
        };

        var result = service.Simulate(args);

        result.Should().ContainInOrder(
            "Class1.this.Method1()",
            "Class2.this.Method2()"
        );
    }
}
