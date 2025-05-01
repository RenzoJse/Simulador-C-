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

    #endregion

    #region Success

    [TestMethod]
    public void CreateValueType_WhenTypeIsValid_ShouldReturnValueType()
    {
        var valueType = new ValueType(_validName, _validType, _emptyMethods);

        valueType.Should().NotBeNull();
        valueType.Type.Should().Be(_validType);
    }

    #endregion

    #endregion
}
