using FluentAssertions;

namespace ObjectSim.Domain.Test;
[TestClass]
public class AttributeTest
{
    [TestMethod]
    public void DataType_Property_SetAndGet_ShouldBeEqual()
    {
        var attribute = new Attribute();
        attribute.DataType = Attribute.AttributeDataType.Decimal;
        attribute.DataType.Should().Be(Attribute.AttributeDataType.Decimal);
    }
    [TestMethod]
    public void Visibility_Property_SetAndGet_ShouldBeEqual()
    {
        var attribute = new Attribute();

        attribute.Visibility = Attribute.AttributeVisibility.ProtectedInternal;

        attribute.Visibility.Should().Be(Attribute.AttributeVisibility.ProtectedInternal);
    }
    [TestMethod]
    public void Name_Property_SetAndGet_ShouldBeEqual()
    {
        var attribute = new Attribute();
        attribute.Name = "TestAttribute";
        attribute.Name.Should().Be("TestAttribute");
    }
    [TestMethod]
    public void Id_Property_SetAndGet_ShouldBeEqual()
    {
        var id = Guid.NewGuid();
        var attribute = new Attribute();
        attribute.Id = id;
        attribute.Id.Should().Be(id);
    }
    [TestMethod]
    public void AttributeDataTypeCreateAttribute_OKTest()
    {
        var attribute = new Attribute();
        attribute.DataType = Attribute.AttributeDataType.String;
        Assert.AreEqual(Attribute.AttributeDataType.String, attribute.DataType);
    }
    [TestMethod]
    public void AttributeVisibilityCreateAttribute_OKTest()
    {
        var attribute = new Attribute();
        attribute.Visibility = Attribute.AttributeVisibility.Public;
        Assert.AreEqual(Attribute.AttributeVisibility.Public, attribute.Visibility);
    }
    [TestMethod]
    public void AttributeName_CreateAttribute_OKTest()
    {
        var attribute = new Attribute();
        attribute.Name = "TestAttribute";
        Assert.AreEqual("TestAttribute", attribute.Name);
    }
    [TestMethod]
    public void Validate_ValidAttribute_ShouldNotThrow()
    {
        var attribute = new Attribute
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            DataType = Attribute.AttributeDataType.String,
            Visibility = Attribute.AttributeVisibility.Public
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
            DataType = Attribute.AttributeDataType.String,
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
            DataType = Attribute.AttributeDataType.String,
            Visibility = Attribute.AttributeVisibility.Public
        };

        Action act = attribute.Validate;

        act.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot be null or whitespace.");
    }
    [TestMethod]
    public void Validate_ShouldThrow_WhenNameIsWhitespace()
    {
        var attribute = new Attribute
        {
            Id = Guid.NewGuid(),
            Name = "   ",
            DataType = Attribute.AttributeDataType.String,
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
            DataType = Attribute.AttributeDataType.String,
            Visibility = Attribute.AttributeVisibility.Public
        };

        Action act = attribute.Validate;

        act.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot exceed 100 characters.");
    }
    [TestMethod]
    public void Validate_ShouldNotThrow_WhenNameIsOk()
    {
        var attribute = new Attribute
        {
            Id = Guid.NewGuid(),
            Name = "aaa",
            DataType = Attribute.AttributeDataType.String,
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
            DataType = Attribute.AttributeDataType.Int,
            Visibility = Attribute.AttributeVisibility.Public
        };

        Action act = attribute.Validate;

        act.Should().NotThrow();
    }
    [TestMethod]
    public void AttributeValidator_WithInvalidDataType_ShouldThrowArgumentException()
    {
        var attribute = new Attribute
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            Visibility = Attribute.AttributeVisibility.Public,
            DataType = (Attribute.AttributeDataType)999
        };

        Action act = attribute.Validate;

        act.Should().Throw<ArgumentException>()
            .WithMessage("Invalid data type.");
    }
    [TestMethod]
    public void AttributeValidator_WithInvalidVisibility_ShouldThrowArgumentException()
    {
        var attribute = new Attribute
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            DataType = Attribute.AttributeDataType.String,
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
            DataType = Attribute.AttributeDataType.String,
            Visibility = Attribute.AttributeVisibility.Internal
        };

        Action act = attribute.Validate;

        act.Should().NotThrow();
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ValueType_ShouldThrow_WhenInvalidType()
    {
        var type = new ValueType("invalid");
    }
}
