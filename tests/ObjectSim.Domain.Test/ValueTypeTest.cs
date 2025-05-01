using FluentAssertions;

namespace ObjectSim.Domain.Test;

[TestClass]
public class ValueTypeTest
{
    private string _validType = null!;
    private string _validName = null!;
    private List<Method> _emptyMethods = null!;
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
            var valueType = new ValueType(_validName, invalidType, _emptyMethods);
        };

        action.Should().Throw<ArgumentException>()
            .WithMessage($"Invalid ValueType: {invalidType}.");
    }

    [TestMethod]
    public void CreateValueType_WhenNameIsNull_ShouldThrowArgumentException()
    {
        string name = null!;

        Action action = () =>
        {
            var valueType = new ValueType(name, _validType, _emptyMethods);
        };

        action.Should().Throw<ArgumentNullException>()
            .WithMessage("Name cannot be null or empty. (Parameter 'name')");
    }
    
    [TestMethod]
    public void CreateValueType_WhenNameIsTooLong_ShouldThrowArgumentException()
    {
        var name = new string('a', 21);

        Action action = () =>
        {
            var valueType = new ValueType(name, _validType, _emptyMethods);
        };

        action.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot be longer than 20 characters.");
    }
    
    [TestMethod]
    public void CreateValueType_WhenNameHasSymbols_ShouldThrowArgumentException()
    {
        const string name = "Invalid@Name";
        
        Action action = () =>
        {
            var valueType = new ValueType(name, _validType, _emptyMethods);
        };

        action.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot contain special characters.");
    }
    
    [TestMethod]
    public void CreateValueType_WhenNameHasNumbers_ShouldThrowArgumentException()
    {
        const string name = "Invalid123Name";

        Action action = () =>
        {
            var valueType = new ValueType(name, _validType, _emptyMethods);
        };

        action.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot contain special characters.");
    }
    
    [TestMethod]
    public void CreateValueType_WhenNameHasSpaces_ShouldThrowArgumentException()
    {
        const string name = "Invalid123Name";

        Action action = () =>
        {
            var valueType = new ValueType(name, _validType, _emptyMethods);
        };

        action.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot contain special characters.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void CreateValueType_WhenTypeIsValid_ShouldReturnValueType()
    {
        var valueType = new ValueType(_validName, _validType, _emptyMethods);

        valueType.Should().NotBeNull();
        valueType.Type.Should().Be(_validType);
    }
    
    [TestMethod]
    public void CreateValueType_WhenNameIsValid_ShouldReturnValueType()
    {
        var valueType = new ValueType(_validName, _validType, _emptyMethods);

        valueType.Should().NotBeNull();
        valueType.Name.Should().Be(_validName);
        valueType.Type.Should().Be(_validType);
        valueType.Methods.Should().BeEmpty();
        valueType.MethodIds.Should().BeEmpty();
    }

    #endregion

    #endregion
}
