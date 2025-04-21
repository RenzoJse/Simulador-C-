using FluentAssertions;

namespace ObjectSim.Domain.Test;

[TestClass]
public class MethodTest
{
    [TestMethod]
    public void DataType_Property_SetAndGet_ShouldBeEqual()
    {
        var method = new Method();
        method.Type = Method.MethodDataType.Decimal;
        method.Type.Should().Be(Method.MethodDataType.Decimal);
    }

    [TestMethod]
    public void Visibility_Property_SetAndGet_ShouldBeEqual()
    {
        var method = new Method();

        method.Accessibility = Method.MethodAccessibility.ProtectedInternal;

        method.Accessibility.Should().Be(Method.MethodAccessibility.ProtectedInternal);
    }

    [TestMethod]
    public void MethodDataTypeCreateMethod_OKTest()
    {
        var method = new Method();
        method.Type = Method.MethodDataType.String;
        Assert.AreEqual(Method.MethodDataType.String, method.Type);
    }
    [TestMethod]
    public void MethodVisibilityCreateMethod_OKTest()
    {
        var method = new Method();
        method.Accessibility = Method.MethodAccessibility.Public;
        Assert.AreEqual(Method.MethodAccessibility.Public, method.Accessibility);
    }

    [TestMethod]
    public void Name_Property_SetAndGet_ShouldBeEqual()
    {
        var method = new Method();
        method.Name = "TestMethod";
        method.Name.Should().Be("TestMethod");
    }

    [TestMethod]
    public void Id_Property_SetAndGet_ShouldBeEqual()
    {
        var id = Guid.NewGuid();
        var method = new Method();
        method.Id = id;
        method.Id.Should().Be(id);
    }

    [TestMethod]
    public void MethodName_CreateAttribute_OKTest()
    {
        var method = new Method();
        method.Name = "TestMethod";
        Assert.AreEqual("Testmethod", method.Name);
    }

    [TestMethod]
    public void Validate_ValidMethod_ShouldNotThrow()
    {
        var method = new Method
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            Type = Method.MethodDataType.String,
            Accessibility = Method.MethodAccessibility.Public
        };

        Action act = method.ValidateFields;

        act.Should().NotThrow();
    }

    [TestMethod]
    public void Validate_ShouldThrow_WhenIdIsEmpty()
    {
        var method = new Method
        {
            Id = Guid.Empty,
            Name = "Test",
            Type = Method.MethodDataType.String,
            Accessibility = Method.MethodAccessibility.Public
        };

        Action act = method.ValidateFields;

        act.Should().Throw<ArgumentException>()
            .WithMessage("Id must be a valid non-empty GUID.");
    }

    [TestMethod]
    public void Validate_ShouldThrow_WhenNameIsNull()
    {
        var method = new Method
        {
            Id = Guid.NewGuid(),
            Name = string.Empty,
            Type = Method.MethodDataType.String,
            Accessibility = Method.MethodAccessibility.Public
        };

        Action act = method.ValidateFields;

        act.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot be null or whitespace.");
    }
    [TestMethod]
    public void Validate_ShouldThrow_WhenNameIsWhiteSpace()
    {
        var method = new Method
        {
            Id = Guid.NewGuid(),
            Name = " ",
            Type = Method.MethodDataType.String,
            Accessibility = Method.MethodAccessibility.Public
        };

        Action act = method.ValidateFields;

        act.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot be null or whitespace.");
    }

    [TestMethod]
    public void Validate_ShouldThrow_WhenNameStartWithANum()
    {
        var method = new Method
        {
            Id = Guid.NewGuid(),
            Name = "1Test",
            Type = Method.MethodDataType.String,
            Accessibility = Method.MethodAccessibility.Public
        };

        Action act = method.ValidateFields;

        act.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot be null or start with a num.");
    }

    [TestMethod]
    public void Validate_ShouldThrow_WhenNameIsTooLong()
    {
        var method = new Method
        {
            Id = Guid.NewGuid(),
            Name = new string('t', 105),
            Type = Method.MethodDataType.String,
            Accessibility = Method.MethodAccessibility.Public
        };

        Action act = method.ValidateFields;

        act.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot exceed 100 characters.");
    }

    [TestMethod]
    public void Validate_ShouldNotThrow_WhenNameIsOk()
    {
        var method = new Method
        {
            Id = Guid.NewGuid(),
            Name = "mmmm",
            Type = Method.MethodDataType.String,
            Accessibility = Method.MethodAccessibility.Public
        };

        Action act = method.ValidateFields;

        act.Should().NotThrow();
    }

    [TestMethod]
    public void MethodValidator_WithValidDataType_ShouldNotThrow()
    {
        var method = new Method
        {
            Id = Guid.NewGuid(),
            Name = "mmmm",
            Type = Method.MethodDataType.String,
            Accessibility = Method.MethodAccessibility.Public
        };

        Action act = method.ValidateFields;

        act.Should().NotThrow();
    }

    [TestMethod]
    public void MethodValidator_WithInvalidDataType_ShouldThrowArgumentException()
    {
        var method = new Method
        {
            Id = Guid.NewGuid(),
            Name = "test",
            Type = (Method.MethodDataType)999,
            Accessibility = Method.MethodAccessibility.Public
        };


        Action act = method.ValidateFields;

        act.Should().Throw<ArgumentException>()
            .WithMessage("Invalid data type.");
    }

    [TestMethod]
    public void MethodValidator_WithInvalidVisibility_ShouldThrowArgumentException()
    {
        var method = new Method
        {
            Id = Guid.NewGuid(),
            Name = "test",
            Type = Method.MethodDataType.String,
            Accessibility = (Method.MethodAccessibility)99
        };

        Action act = method.ValidateFields;

        act.Should().Throw<ArgumentException>()
            .WithMessage("Invalid accesibility type.");
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
}
