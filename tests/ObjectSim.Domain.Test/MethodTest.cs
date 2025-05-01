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
    public void Accessibility_Property_SetAndGet_ShouldBeEqual()
    {
        var method = new Method();
        method.Accessibility = Method.MethodAccessibility.ProtectedInternal;
        method.Accessibility.Should().Be(Method.MethodAccessibility.ProtectedInternal);
    }

    [TestMethod]
    public void MethodDataType_CreateMethod_ShouldSetCorrectly()
    {
        var method = new Method();
        method.Type = Method.MethodDataType.String;
        Assert.AreEqual(Method.MethodDataType.String, method.Type);
    }

    [TestMethod]
    public void MethodAccessibility_CreateMethod_ShouldSetCorrectly()
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
        var method = new Method { Id = id };
        method.Id.Should().Be(id);
    }

    [TestMethod]
    public void Name_SetToEmpty_ShouldThrowArgumentException()
    {
        var method = new Method();

        Action act = () => method.Name = string.Empty;

        act.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot be null or whitespace.");
    }

    [TestMethod]
    public void Name_SetToWhiteSpace_ShouldThrowArgumentException()
    {
        var method = new Method();

        Action act = () => method.Name = " ";

        act.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot be null or whitespace.");
    }

    [TestMethod]
    public void Name_SetToTooLongValue_ShouldThrowArgumentException()
    {
        var method = new Method();

        Action act = () => method.Name = new string('t', 105);

        act.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot exceed 100 characters.");
    }

    [TestMethod]
    public void Name_SetToValueStartingWithNumber_ShouldThrowArgumentException()
    {
        var method = new Method();

        Action act = () => method.Name = "1Test";

        act.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot be null or start with a num.");
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
    public void Setting_InvalidDataType_ShouldThrowArgumentException()
    {
        var method = new Method();

        Action act = () => method.Type = (Method.MethodDataType)999;

        act.Should().Throw<ArgumentException>()
            .WithMessage("Invalid data type.");
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
    public void MethodValidator_WithInvalidAccessibility_ShouldThrowArgumentException()
    {
        var method = new Method
        {
            Id = Guid.NewGuid(),
            Name = "test",
            Type = Method.MethodDataType.String
        };

        var accessibilityField = typeof(Method).GetField("_accessibility", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        accessibilityField!.SetValue(method, (Method.MethodAccessibility)99);

        Action act = method.ValidateFields;

        act.Should().Throw<ArgumentException>()
            .WithMessage("Invalid accesibility type.");
    }

    [TestMethod]
    public void MethodValidator_WithValidAccessibility_ShouldNotThrow()
    {
        var method = new Method
        {
            Id = Guid.NewGuid(),
            Name = "test",
            Type = Method.MethodDataType.String,
            Accessibility = Method.MethodAccessibility.Public
        };

        Action act = method.ValidateFields;

        act.Should().NotThrow();
    }
    [TestMethod]
    public void Abstract_SetAndGet_ShouldBeEqual()
    {
        var method = new Method { Abstract = true };
        method.Abstract.Should().BeTrue();
    }

    [TestMethod]
    public void IsSealed_SetAndGet_ShouldBeEqual()
    {
        var method = new Method { IsSealed = true };
        method.IsSealed.Should().BeTrue();
    }

    [TestMethod]
    public void Parameters_AddParameter_ShouldContainParameter()
    {
        var method = new Method();
        var param = new Parameter();
        method.Parameters.Add(param);
        method.Parameters.Should().Contain(param);
    }

    [TestMethod]
    public void LocalVariables_AddLocalVariable_ShouldContainLocalVariable()
    {
        var method = new Method();
        var localVar = new LocalVariable();
        method.LocalVariables.Add(localVar);
        method.LocalVariables.Should().Contain(localVar);
    }

    [TestMethod]
    public void Id_DefaultValue_ShouldNotBeEmpty()
    {
        var method = new Method();
        method.Id.Should().NotBe(Guid.Empty);
    }

    [TestMethod]
    public void ValidateFields_WithInvalidName_ShouldThrow()
    {
        var method = new Method
        {
            Id = Guid.NewGuid(),
            Type = Method.MethodDataType.String,
            Accessibility = Method.MethodAccessibility.Public
        };

        var nameField = typeof(Method).GetField("_name", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        nameField!.SetValue(method, "1Invalid");

        Action act = method.ValidateFields;
        act.Should().Throw<ArgumentException>().WithMessage("Name cannot be null or start with a num.");
    }

    [TestMethod]
    public void MethodValidator_WithInvalidDataType_ShouldThrowArgumentException()
    {
        var method = new Method
        {
            Id = Guid.NewGuid(),
            Name = "test",
            Accessibility = Method.MethodAccessibility.Public
        };

        var typeField = typeof(Method).GetField("_type", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        typeField!.SetValue(method, (Method.MethodDataType)999);

        Action act = method.ValidateFields;

        act.Should().Throw<ArgumentException>()
            .WithMessage("Invalid data type.");
    }

    [TestMethod]
    public void ValidateFields_WithNullName_ShouldThrow()
    {
        var method = new Method
        {
            Id = Guid.NewGuid(),
            Type = Method.MethodDataType.String,
            Accessibility = Method.MethodAccessibility.Public
        };

        var nameField = typeof(Method).GetField("_name", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        nameField!.SetValue(method, null);

        Action act = method.ValidateFields;

        act.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot be null or whitespace.");
    }

    [TestMethod]
    public void Constructor_ShouldInitializeParametersAndLocalVariables()
    {
        var method = new Method();

        method.Parameters.Should().NotBeNull();
        method.LocalVariables.Should().NotBeNull();
    }

    [TestMethod]
    public void Constructor_WhenClassIsSet_ShouldInitializeClass()
    {
        var method = new Method();
        var classInstance = new Class();
        method.Class = classInstance;
        method.ClassId = classInstance.Id;

        method.Class.Should().Be(classInstance);
        method.ClassId.Should().Be(classInstance.Id);
    }
}
