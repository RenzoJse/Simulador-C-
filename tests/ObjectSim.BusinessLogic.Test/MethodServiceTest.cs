using ObjectSim.IDataAccess;
using Moq;
using ObjectSim.Domain;
using System.Linq.Expressions;
using FluentAssertions;

namespace ObjectSim.BusinessLogic.Test;

[TestClass]
public class MethodServiceTest
{
    private Mock<IMethodRepository<Method>>? _methodRepositoryMock;
    private MethodService?  _methodService;

    [TestInitialize]
    public void Initialize()
    {
        _methodRepositoryMock = new Mock<IMethodRepository<Method>>(MockBehavior.Strict);
        _methodService = new MethodService(_methodRepositoryMock.Object);
    }


    [TestMethod]
    public void CreateValidMethod()
    {
        var testMethod = new Method
        {
            Name = "MetodoDePrueba",
            Type = "boolean",
            Abstract = false,
            IsSealed = false,
            Accessibility = "public",
            Parameters = new List<Parameter>(),
            LocalVariables = new List<LocalVariable>()
        };

        _methodRepositoryMock.Setup(repo => repo.Exist(It.IsAny<Expression<Func<Method, bool>>>())).Returns(false);
        _methodRepositoryMock.Setup(repo => repo.Add(It.IsAny<Method>())).Returns((Method act) => act);

        var result = _methodService.Create(testMethod);

        result.Should().NotBeNull();
        _methodRepositoryMock.VerifyAll();
    }
}
