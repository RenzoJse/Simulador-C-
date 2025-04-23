using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using ObjectSim.IDataAccess;

namespace ObjectSim.BusinessLogic.Test;
[TestClass]
public class LocalVariableServiceTest
{
    private Mock<ILocalVariableRepository<Domain.LocalVariable>>? _localVariableRepositoryMock;
    private LocalVariableService? _localVariableService;
    private static readonly Guid ClassId = Guid.NewGuid();
    private Domain.LocalVariable? _localVariable;


    [TestInitialize]
    public void Setup()
    {
        _localVariableRepositoryMock = new Mock<ILocalVariableRepository<Domain.LocalVariable>>(MockBehavior.Strict);
        _localVariableService = new LocalVariableService(_localVariableRepositoryMock.Object);
        _localVariable = new Domain.LocalVariable
        {
            Id = ClassId,
            Name = "LocalVariable1",
            Type = Domain.LocalVariable.LocalVariableDataType.String
        };
    }


    [TestMethod]
    public void CreateValidLocalVariable()
    {

        _localVariableRepositoryMock!.Setup(repo => repo.Exist(It.IsAny<Expression<Func<Domain.LocalVariable, bool>>>())).Returns(false);
        _localVariableRepositoryMock.Setup(repo => repo.Add(It.IsAny<Domain.LocalVariable>())).Returns((Domain.LocalVariable act) => act);

        var result = _localVariableService!.Create(_localVariable!);

        result.Should().NotBeNull();
        _localVariableRepositoryMock.VerifyAll();
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]

    public void CreateNullLocalVariable()
    {
        _localVariableService!.Create(null!);
    }
}
