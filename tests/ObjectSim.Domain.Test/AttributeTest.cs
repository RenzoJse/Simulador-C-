using FluentAssertions;
using Action = System.Action;

namespace ObjectSim.Domain.Test;
[TestClass]
public class AttributeTest
{
    [TestMethod]
    public void DataType_Property_SetAndGet_ShouldBeEqual()
    {
        var expectedType = new ValueType("myVariable", "int");
        var attribute = new Attribute { DataType = expectedType };

        attribute.DataType.Should().Be(expectedType);
        attribute.DataType.Name.Should().Be("myVariable");
        attribute.DataType.Type.Should().Be("int");
    }

    [TestMethod]
    public void Validate_ShouldThrow_WhenDataTypeIsNull()
    {
        var attribute = new Attribute
        {
            Id = Guid.NewGuid(),
            Name = "Age",
            ClassId = Guid.NewGuid(),
            DataType = null!,
            Visibility = Attribute.AttributeVisibility.Public
        };

        Action act = attribute.Validate;

        act.Should().Throw<ArgumentException>()
            .WithMessage("DataType is required.");
    }

    [TestMethod]
    public void ValidateDataType_ShouldPass_WhenValidValueType()
    {
        var valid = new ValueType("myVariable", "int");

        Attribute.ValidateDataType(valid);
    }

    [TestMethod]
    public void DataType_Property_SetAndGet_ShouldBeEqualReference()
    {
        var expectedType = new ReferenceType("myString", "string");
        var attribute = new Attribute { DataType = expectedType };

        attribute.DataType = expectedType;
        attribute.DataType.Should().NotBeNull();
        attribute.DataType.Should().Be(expectedType);
        attribute.DataType.Name.Should().NotBeNullOrEmpty();
        attribute.DataType.Type.Should().Be("string");
        attribute.DataType.Name.Should().Be("myString");
    }

    [TestMethod]
    public void Visibility_Property_SetAndGet_ShouldBeEqual()
    {
        var attribute = new Attribute { Visibility = Attribute.AttributeVisibility.Protected };

        attribute.Visibility.Should().Be(Attribute.AttributeVisibility.Protected);
    }

    [TestMethod]
    public void Name_Property_SetAndGet_ShouldBeEqual()
    {
        var attribute = new Attribute { Name = "TestAttribute" };
        attribute.Name.Should().Be("TestAttribute");
    }

    [TestMethod]
    public void Id_Property_SetAndGet_ShouldBeEqual()
    {
        var id = Guid.NewGuid();
        var attribute = new Attribute { Id = id };
        attribute.Id.Should().Be(id);
    }

    [TestMethod]
    public void Validate_ValidAttribute_ShouldNotThrow()
    {
        var attribute = new Attribute
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            DataType = new ValueType("myVariable", "int"),
            ClassId = Guid.NewGuid(),
            Visibility = Attribute.AttributeVisibility.Public
        };

        Action action = attribute.Validate;

        action.Should().NotThrow();
    }

    [TestMethod]
    public void AttributeDataTypeCreateAttributeValue_OKTest()
    {
        var attr = new Attribute
        {
            Id = Guid.NewGuid(),
            Name = "Age",
            ClassId = Guid.NewGuid(),
            Visibility = Attribute.AttributeVisibility.Public,
            DataType = new ValueType("myVariable", "int")
        };

        Action action = attr.Validate;
        action.Should().NotThrow();
    }

    [TestMethod]
    public void AttributeDataTypeCreateAttributeReference_OKTest()
    {
        var attr = new Attribute
        {
            Id = Guid.NewGuid(),
            Name = "Age",
            ClassId = Guid.NewGuid(),
            Visibility = Attribute.AttributeVisibility.Public,
            DataType = new ReferenceType("myString", "string")
        };

        Action action = attr.Validate;
        action.Should().NotThrow();
    }

    [TestMethod]
    public void AttributeVisibilityCreateAttribute_OKTest()
    {
        var attribute = new Attribute { Visibility = Attribute.AttributeVisibility.Public };
        Assert.AreEqual(Attribute.AttributeVisibility.Public, attribute.Visibility);
    }

    [TestMethod]
    public void AttributeName_CreateAttribute_OKTest()
    {
        var attribute = new Attribute { Name = "TestAttribute" };
        Assert.AreEqual("TestAttribute", attribute.Name);
    }

    [TestMethod]
    public void Validate_ValidAttributeReference_ShouldNotThrow()
    {
        var attribute = new Attribute
        {
            Id = Guid.NewGuid(),
            Name = "Name",
            ClassId = Guid.NewGuid(),
            DataType = new ReferenceType("myString", "string"),
            Visibility = Attribute.AttributeVisibility.Private
        };

        Action act = attribute.Validate;

        act.Should().NotThrow();
    }

    [TestMethod]
    public void Validate_ValidAttributeValue_ShouldNotThrow()
    {
        var attribute = new Attribute
        {
            Id = Guid.NewGuid(),
            Name = "Name",
            ClassId = Guid.NewGuid(),
            DataType = new ValueType("myVariable", "int"),
            Visibility = Attribute.AttributeVisibility.Private
        };

        Action act = attribute.Validate;

        act.Should().NotThrow();
    }

    [TestMethod]
    public void Validate_ShouldThrow_WhenIdIsEmpty()
    {
        var attribute = new Attribute
        {
            Id = Guid.Empty,
            Name = "ValidAttribute",
            DataType = new ReferenceType("myString", "string"),
            Visibility = Attribute.AttributeVisibility.Public
        };

        Action act = attribute.Validate;

        act.Should().Throw<ArgumentException>()
            .WithMessage("Id must be a valid non-empty GUID.");
    }

    [TestMethod]
    public void Validate_ShouldThrow_WhenIdIsEmptyValue()
    {
        var attribute = new Attribute
        {
            Id = Guid.Empty,
            Name = "ValidAttribute",
            DataType = new ValueType("myVariable", "int"),
            Visibility = Attribute.AttributeVisibility.Public
        };

        Action act = attribute.Validate;

        act.Should().Throw<ArgumentException>()
            .WithMessage("Id must be a valid non-empty GUID.");
    }

    [TestMethod]
    public void Validate_ShouldThrow_WhenNameIsNull()
    {
        var attribute = new Attribute
        {
            Id = Guid.NewGuid(),
            Name = null,
            ClassId = Guid.NewGuid(),
            DataType = new ValueType("myVariable", "int"),
            Visibility = Attribute.AttributeVisibility.Public
        };

        Action act = attribute.Validate;

        act.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot be null or whitespace.");
    }

    [TestMethod]
    public void Validate_ShouldThrowReference_WhenNameIsNull()
    {
        var attribute = new Attribute
        {
            Id = Guid.NewGuid(),
            Name = null,
            ClassId = Guid.NewGuid(),
            DataType = new ReferenceType("myString", "string"),
            Visibility = Attribute.AttributeVisibility.Public
        };

        Action act = attribute.Validate;

        act.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot be null or whitespace.");
    }

    [TestMethod]
    public void Validate_ShouldThrow_WhenNameIsWhitespaceWithReference()
    {
        var attribute = new Attribute
        {
            Id = Guid.NewGuid(),
            Name = "   ",
            ClassId = Guid.NewGuid(),
            DataType = new ReferenceType("myString", "string"),
            Visibility = Attribute.AttributeVisibility.Public
        };

        Action act = attribute.Validate;

        act.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot be null or whitespace.");
    }

    [TestMethod]
    public void Validate_ShouldThrow_WhenNameIsWhitespaceWithValue()
    {
        var attribute = new Attribute
        {
            Id = Guid.NewGuid(),
            Name = "   ",
            ClassId = Guid.NewGuid(),
            DataType = new ValueType("myVariable", "int"),
            Visibility = Attribute.AttributeVisibility.Public
        };

        Action act = attribute.Validate;

        act.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot be null or whitespace.");
    }

    [TestMethod]
    public void Validate_ShouldThrow_WhenNameIsTooLong()
    {
        var attribute = new Attribute
        {
            Id = Guid.NewGuid(),
            Name = new string('a', 101),
            ClassId = Guid.NewGuid(),
            DataType = new ValueType("myVariable", "int"),
            Visibility = Attribute.AttributeVisibility.Public
        };

        Action act = attribute.Validate;

        act.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot be less than 1 or more than 10 characters.");
    }

    [TestMethod]
    public void Validate_ShouldNotThrow_WhenNameIsOk()
    {
        var attribute = new Attribute
        {
            Id = Guid.NewGuid(),
            Name = "aaa",
            ClassId = Guid.NewGuid(),
            DataType = new ValueType("myVariable", "int"),
            Visibility = Attribute.AttributeVisibility.Public
        };

        Action act = attribute.Validate;

        act.Should().NotThrow();
    }

    [TestMethod]
    public void AttributeValidator_WithValidDataType_ShouldNotThrow()
    {
        var attribute = new Attribute
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            ClassId = Guid.NewGuid(),
            DataType = new ValueType("myVariable", "int"),
            Visibility = Attribute.AttributeVisibility.Public
        };

        Action act = attribute.Validate;

        act.Should().NotThrow();
    }

    [TestMethod]
    public void AttributeValidator_WithValidReferenceType_ShouldNotThrow()
    {
        var attribute = new Attribute
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            ClassId = Guid.NewGuid(),
            DataType = new ReferenceType("myString", "string"),
            Visibility = Attribute.AttributeVisibility.Public
        };

        Action act = attribute.Validate;

        act.Should().NotThrow();
    }

    [TestMethod]
    public void AttributeValidator_WithInvalidDataType_ShouldThrowArgumentException()
    {
        Action act = () =>
        {
            var valueType = new ValueType("myVariable", "false");
        };

        act.Should().Throw<ArgumentException>()
            .WithMessage("Invalid ValueType: false.");
    }

    [TestMethod]
    public void AttributeValidator_WithInvalidVisibility_ShouldThrowArgumentException()
    {
        var attribute = new Attribute
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            ClassId = Guid.NewGuid(),
            DataType = new ValueType("myVariable", "int"),
            Visibility = (Attribute.AttributeVisibility)999
        };

        Action act = attribute.Validate;

        act.Should().Throw<ArgumentException>()
            .WithMessage("Invalid visibility type.");
    }

    [TestMethod]
    public void AttributeValidator_WithValidVisibility_ShouldNotThrow()
    {
        var attribute = new Attribute
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            ClassId = Guid.NewGuid(),
            DataType = new ValueType("myVariable", "int"),
            Visibility = Attribute.AttributeVisibility.Internal
        };

        Action act = attribute.Validate;

        act.Should().NotThrow();
    }

    [TestMethod]
    public void AttributeValidator_WithValidVisibility_ShouldNotThrowReference()
    {
        var attribute = new Attribute
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            ClassId = Guid.NewGuid(),
            DataType = new ReferenceType("myString", "string"),
            Visibility = Attribute.AttributeVisibility.Internal
        };

        Action act = attribute.Validate;

        act.Should().NotThrow();
    }

    [TestMethod]
    public void Validate_ShouldThrow_WhenNameStartWithANum()
    {
        var attribute = new Attribute
        {
            Id = Guid.NewGuid(),
            Name = "1Test1",
            ClassId = Guid.NewGuid(),
            DataType = new ReferenceType("myString", "string"),
            Visibility = Attribute.AttributeVisibility.Internal
        };
        Action act = attribute.Validate;

        act.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot be null or start with a num.");
    }

    [TestMethod]
    public void Attribute_ShouldHaveCorrectClassId()
    {
        var classId = Guid.NewGuid();

        var attribute = new Attribute
        {
            Id = Guid.NewGuid(),
            Name = "TestAttribute",
            DataType = new ReferenceType("myString", "string"),
            Visibility = Attribute.AttributeVisibility.Public,
            ClassId = classId
        };
        attribute.ClassId.Should().Be(classId);
    }

    [TestMethod]
    public void Validate_ShouldThrow_WhenClassIdIsEmpty()
    {
        var attribute = new Attribute
        {
            Id = Guid.NewGuid(),
            ClassId = Guid.Empty,
            Name = "ValidName",
            DataType = new ReferenceType("myString", "string"),
            Visibility = Attribute.AttributeVisibility.Public
        };

        Action act = attribute.Validate;

        act.Should().Throw<ArgumentException>()
            .WithMessage("Id must be a valid non-empty GUID.");
    }
}
