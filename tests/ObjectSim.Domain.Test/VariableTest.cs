using FluentAssertions;

namespace ObjectSim.Domain.Test;

[TestClass]
public class VariableTest
{
    private string _validName = null!;
    private Guid _referenceId;

    [TestInitialize]
    public void Initialize()
    {
        _validName = "ValidVariableName";
        _referenceId = Guid.NewGuid();
    }

    [TestCleanup]
    public void Cleanup()
    {
    }

    #region CreateVariable

    #region Error

    [TestMethod]
    public void CreateVariable_WhenNameIsShort_ThrowsArgumentException()
    {
        const string shortName = "a";
        Action action = () =>
        {
            var variable = new Variable(_referenceId, shortName);
        };
        action.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot be that short. (Parameter 'name')");
    }

    [TestMethod]
    public void CreateVariable_WhenNameIsLong_ThrowsArgumentException()
    {
        const string longName = "ThisNameIsWayTooLongForAVariable";
        Action action = () =>
        {
            var variable = new Variable(_referenceId, longName);
        };
        action.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot be longer than 20 characters. (Parameter 'name')");
    }

    [TestMethod]
    public void CreateVariable_WhenNameIsNull_ThrowsArgumentException()
    {
        string? nullName = null;
        Action action = () =>
        {
            var variable = new Variable(_referenceId, nullName!);
        };
        action.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot be null or whitespace. (Parameter 'name')");
    }

    [TestMethod]
    public void CreateVariable_WhenNameIsWhitespace_ThrowsArgumentException()
    {
        const string whitespaceName = "   ";
        Action action = () =>
        {
            var variable = new Variable(_referenceId, whitespaceName);
        };
        action.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot be null or whitespace. (Parameter 'name')");
    }

    [TestMethod]
    public void CreateVariable_WhenDataTypeIdIsEmpty_ThrowsArgumentException()
    {
        var emptyGuid = Guid.Empty;
        Action action = () =>
        {
            var variable = new Variable(emptyGuid, _validName);
        };
        action.Should().Throw<ArgumentException>()
            .WithMessage("Data type ID cannot be empty. (Parameter 'dataTypeId')");
    }

    #endregion

    #region Success

    [TestMethod]
    public void CreateVariable_WithValidName_CreatesVariable()
    {
        const string validName = "ValidName";
        var variable = new Variable(_referenceId, validName);

        variable.Should().NotBeNull();
        variable.Name.Should().Be(validName);
        variable.TypeId.Should().Be(_referenceId);
        variable.VariableId.Should().NotBe(Guid.Empty);
    }

    [TestMethod]
    public void CreateVariable_WithNameOfLength3_CreatesVariable()
    {
        const string validName = "abc";
        var variable = new Variable(_referenceId, validName);

        variable.Name.Should().Be(validName);
    }

    [TestMethod]
    public void CreateVariable_WithNameOfLength20_CreatesVariable()
    {
        const string validName = "abcdefghijklmnopqrst";
        var variable = new Variable(_referenceId, validName);

        variable.Name.Should().Be(validName);
    }

    #endregion

    #endregion

}
