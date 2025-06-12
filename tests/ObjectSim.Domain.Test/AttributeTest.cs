using FluentAssertions;
using Action = System.Action;

namespace ObjectSim.Domain.Test;

[TestClass]
public class AttributeTest
{
    private ValueType _valueType = null!;
    private ReferenceType _referenceType = null!;
    private Guid _classId;
    private Guid _id;
    private Attribute _attribute = null!;

    [TestInitialize]
    public void Setup()
    {
        _valueType = new ValueType(Guid.NewGuid(), "int");
        _referenceType = new ReferenceType(Guid.NewGuid(), "string");
        _classId = Guid.NewGuid();
        _id = Guid.NewGuid();
        _attribute = new Attribute
        {
            Id = _id,
            Name = "TestAtt",
            ClassId = _classId,
            DataType = _valueType,
            Visibility = Attribute.AttributeVisibility.Public
        };
    }

    #region DataType

    [TestMethod]
    public void DataType_Property_SetAndGet_ShouldBeEqual()
    {
        _attribute.DataType = _valueType;
        _attribute.DataType.Should().Be(_valueType);
        _attribute.DataType.Type.Should().Be("int");
    }

    [TestMethod]
    public void DataType_Property_SetAndGet_ShouldBeEqualReference()
    {
        _attribute.DataType = _referenceType;
        _attribute.DataType.Should().NotBeNull();
        _attribute.DataType.Should().Be(_referenceType);
        _attribute.DataType.Type.Should().Be("string");
    }

    [TestMethod]
    public void ValidateDataType_ShouldPass_WhenValidValueType()
    {
        Attribute.ValidateDataType(_valueType);
    }

    [TestMethod]
    public void Validate_ShouldThrow_WhenDataTypeIsNull()
    {
        _attribute.DataType = null!;
        Action act = _attribute.Validate;
        act.Should().Throw<ArgumentException>()
            .WithMessage("DataType is required.");
    }

    #endregion

    #region Visibility

    [TestMethod]
    public void Visibility_Property_SetAndGet_ShouldBeEqual()
    {
        _attribute.Visibility = Attribute.AttributeVisibility.Protected;
        _attribute.Visibility.Should().Be(Attribute.AttributeVisibility.Protected);
    }

    [TestMethod]
    public void AttributeVisibilityCreateAttribute_OKTest()
    {
        _attribute.Visibility = Attribute.AttributeVisibility.Public;
        Assert.AreEqual(Attribute.AttributeVisibility.Public, _attribute.Visibility);
    }

    #endregion

    #region Name

    [TestMethod]
    public void Name_Property_SetAndGet_ShouldBeEqual()
    {
        _attribute.Name = "TestAttribute";
        _attribute.Name.Should().Be("TestAttribute");
    }

    [TestMethod]
    public void AttributeName_CreateAttribute_OKTest()
    {
        _attribute.Name = "TestAttribute";
        Assert.AreEqual("TestAttribute", _attribute.Name);
    }

    #endregion

    #region Id

    [TestMethod]
    public void Id_Property_SetAndGet_ShouldBeEqual()
    {
        _attribute.Id = _id;
        _attribute.Id.Should().Be(_id);
    }

    #endregion

    #region Validate

    #region Error

    [TestMethod]
    public void Validate_ShouldThrow_WhenIdIsEmpty()
    {
        _attribute.Id = Guid.Empty;
        Action act = _attribute.Validate;
        act.Should().Throw<ArgumentException>()
            .WithMessage("Id must be a valid non-empty GUID.");
    }

    [TestMethod]
    public void Validate_ShouldThrow_WhenIdIsEmptyValue()
    {
        _attribute.Id = Guid.Empty;
        _attribute.DataType = _valueType;
        Action act = _attribute.Validate;
        act.Should().Throw<ArgumentException>()
            .WithMessage("Id must be a valid non-empty GUID.");
    }

    [TestMethod]
    public void Validate_ShouldThrow_WhenNameIsNull()
    {
        _attribute.Name = null;
        Action act = _attribute.Validate;
        act.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot be null or whitespace.");
    }

    [TestMethod]
    public void Validate_ShouldThrowReference_WhenNameIsNull()
    {
        _attribute.Name = null;
        _attribute.DataType = _referenceType;
        Action act = _attribute.Validate;
        act.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot be null or whitespace.");
    }

    [TestMethod]
    public void Validate_ShouldThrow_WhenNameIsWhitespaceWithReference()
    {
        _attribute.Name = "   ";
        _attribute.DataType = _referenceType;
        Action act = _attribute.Validate;
        act.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot be null or whitespace.");
    }

    [TestMethod]
    public void Validate_ShouldThrow_WhenNameIsWhitespaceWithValue()
    {
        _attribute.Name = "   ";
        _attribute.DataType = _valueType;
        Action act = _attribute.Validate;
        act.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot be null or whitespace.");
    }

    [TestMethod]
    public void Validate_ShouldThrow_WhenNameIsTooLong()
    {
        _attribute.Name = new string('a', 101);
        _attribute.DataType = _valueType;
        Action act = _attribute.Validate;
        act.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot be less than 1 or more than 10 characters.");
    }

    [TestMethod]
    public void AttributeValidator_WithInvalidDataType_ShouldThrowArgumentException()
    {
        Action act = () =>
        {
            var valueType = new ValueType(Guid.NewGuid(), "false");
        };
        act.Should().Throw<ArgumentException>()
            .WithMessage("Invalid ValueType: false.");
    }

    [TestMethod]
    public void AttributeValidator_WithInvalidVisibility_ShouldThrowArgumentException()
    {
        _attribute.Visibility = (Attribute.AttributeVisibility)999;
        Action act = _attribute.Validate;
        act.Should().Throw<ArgumentException>()
            .WithMessage("Invalid visibility type.");
    }

    [TestMethod]
    public void Validate_ShouldThrow_WhenNameStartWithANum()
    {
        _attribute.Name = "1Test1";
        _attribute.DataType = _referenceType;
        _attribute.Visibility = Attribute.AttributeVisibility.Internal;
        Action act = _attribute.Validate;
        act.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot be null or start with a num.");
    }

    [TestMethod]
    public void Attribute_ShouldHaveCorrectClassId()
    {
        _attribute.ClassId = _classId;
        _attribute.ClassId.Should().Be(_classId);
    }

    [TestMethod]
    public void Validate_ShouldThrow_WhenClassIdIsEmpty()
    {
        _attribute.ClassId = Guid.Empty;
        Action act = _attribute.Validate;
        act.Should().Throw<ArgumentException>()
            .WithMessage("Id must be a valid non-empty GUID.");
    }

    #endregion


    #region Success

    [TestMethod]
    public void Validate_ValidAttribute_ShouldNotThrow()
    {
        Action action = _attribute.Validate;
        action.Should().NotThrow();
    }

    [TestMethod]
    public void AttributeDataTypeCreateAttributeValue_OKTest()
    {
        _attribute.DataType = _valueType;
        Action action = _attribute.Validate;
        action.Should().NotThrow();
    }

    [TestMethod]
    public void AttributeDataTypeCreateAttributeReference_OKTest()
    {
        _attribute.DataType = _referenceType;
        Action action = _attribute.Validate;
        action.Should().NotThrow();
    }

    [TestMethod]
    public void Validate_ValidAttributeReference_ShouldNotThrow()
    {
        _attribute.DataType = _referenceType;
        _attribute.Visibility = Attribute.AttributeVisibility.Private;
        Action act = _attribute.Validate;
        act.Should().NotThrow();
    }

    [TestMethod]
    public void Validate_ValidAttributeValue_ShouldNotThrow()
    {
        _attribute.DataType = _valueType;
        _attribute.Visibility = Attribute.AttributeVisibility.Private;
        Action act = _attribute.Validate;
        act.Should().NotThrow();
    }

    [TestMethod]
    public void Validate_ShouldNotThrow_WhenNameIsOk()
    {
        _attribute.Name = "aaa";
        Action act = _attribute.Validate;
        act.Should().NotThrow();
    }

    [TestMethod]
    public void AttributeValidator_WithValidDataType_ShouldNotThrow()
    {
        _attribute.DataType = _valueType;
        Action act = _attribute.Validate;
        act.Should().NotThrow();
    }

    [TestMethod]
    public void AttributeValidator_WithValidReferenceType_ShouldNotThrow()
    {
        _attribute.DataType = _referenceType;
        Action act = _attribute.Validate;
        act.Should().NotThrow();
    }

    [TestMethod]
    public void AttributeValidator_WithValidVisibility_ShouldNotThrow()
    {
        _attribute.Visibility = Attribute.AttributeVisibility.Internal;
        Action act = _attribute.Validate;
        act.Should().NotThrow();
    }

    [TestMethod]
    public void AttributeValidator_WithValidVisibility_ShouldNotThrowReference()
    {
        _attribute.DataType = _referenceType;
        _attribute.Visibility = Attribute.AttributeVisibility.Internal;
        Action act = _attribute.Validate;
        act.Should().NotThrow();
    }

    #endregion

    #endregion

}
