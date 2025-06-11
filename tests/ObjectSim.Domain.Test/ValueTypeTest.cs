using FluentAssertions;

namespace ObjectSim.Domain.Test;

[TestClass]
public class ValueTypeTest
{
    private string _validType = null!;
    private string _validName = null!;
    private List<Guid> _emptyMethods = null!;
    private List<Method> _methods = null!;

    [TestInitialize]
    public void Initialize()
    {
        _validType = "int";
        _validName = "ValidName";

        _methods =
        [
            new Method(),
            new Method()
        ];

        _emptyMethods = [];
    }

    [TestCleanup]
    public void Cleanup()
    {
    }

    #region CreateValueType

    #region Error

    [TestMethod]
    public void CreateValueType_WhenTypeIsInvalid_ThrowsArgumentException()
    {
        const string invalidType = "invalidType";

        Action action = () =>
        {
            var valueType = new ValueType(Guid.NewGuid(), invalidType);
        };

        action.Should().Throw<ArgumentException>()
            .WithMessage($"Invalid ValueType: {invalidType}.");
    }

    [TestMethod]
    public void CreateValueType_WhenTypeIsNull_ShouldThrowArgumentException()
    {
        _validType = null!;

        Action action = () =>
        {
            var valueType = new ValueType(Guid.NewGuid(), _validType);
        };

        action.Should().Throw<ArgumentNullException>()
            .WithMessage("Name cannot be null or empty. (Parameter 'name')");
    }

    [TestMethod]
    public void CreateValueType_WhenTypeIsTooLong_ShouldThrowArgumentException()
    {
        var invalidType = new string('a', 21);

        Action action = () =>
        {
            var valueType = new ValueType(Guid.NewGuid(), invalidType);
        };

        action.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot be longer than 20 characters.");
    }

    [TestMethod]
    public void CreateValueType_WhenTypeHasSymbols_ShouldThrowArgumentException()
    {
        const string invalidType = "Invalid@Name";

        Action action = () =>
        {
            var valueType = new ValueType(Guid.NewGuid(), invalidType);
        };

        action.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot contain special characters.");
    }

    [TestMethod]
    public void CreateValueType_WhenTypeHasNumbers_ShouldThrowArgumentException()
    {
        const string invalidType = "Invalid123Name";

        Action action = () =>
        {
            var valueType = new ValueType(Guid.NewGuid(), invalidType);
        };

        action.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot contain special characters.");
    }

    [TestMethod]
    public void CreateValueType_WhenNameHasSpaces_ShouldThrowArgumentException()
    {
        const string invalidType = "Invalid123Name";

        Action action = () =>
        {
            var valueType = new ValueType(Guid.NewGuid(), invalidType);
        };

        action.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot contain special characters.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void CreateValueType_WhenTypeIsValid_ShouldReturnValueType()
    {
        var valueType = new ValueType(Guid.NewGuid(), _validType);

        valueType.Should().NotBeNull();
        valueType.Type.Should().Be(_validType);
    }

    [TestMethod]
    public void CreateValueType_WhenNameIsValid_ShouldReturnValueType()
    {
        var valueType = new ValueType(Guid.NewGuid(), _validType);

        valueType.Should().NotBeNull();
        valueType.Type.Should().Be(_validType);
    }

    [TestMethod]
    public void ValueType_DefaultConstructor_ShouldInitializeProperties()
    {
        var valueType = new ValueType();
        valueType.Id.Should().NotBeEmpty();
        valueType.Type.Should().BeEmpty();
    }

    #endregion

    #endregion
}
