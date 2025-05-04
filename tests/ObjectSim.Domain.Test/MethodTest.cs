using FluentAssertions;

namespace ObjectSim.Domain.Test;

[TestClass]
public class MethodTest
{
    private readonly DataType _methodType = new ValueType("methodType", "int", []);
    private readonly DataType _methodReferenceType = new ReferenceType("methodReferenceType", "object", []);

    [TestMethod]
    public void DataType_Property_SetAndGet_ShouldBeEqual()
    {
        var method = new Method { Type = _methodType };
        method.Type.Should().Be(_methodType);
    }

    [TestMethod]
    public void Accessibility_Property_SetAndGet_ShouldBeEqual()
    {
        var method = new Method { Accessibility = Method.MethodAccessibility.ProtectedInternal };
        method.Accessibility.Should().Be(Method.MethodAccessibility.ProtectedInternal);
    }

    [TestMethod]
    public void MethodDataType_CreateMethod_ShouldSetCorrectly()
    {
        var method = new Method { Type = _methodReferenceType };
        Assert.AreEqual(_methodReferenceType, method.Type);
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

        Action act = () => method.Name = new string('t', 52);

        act.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot exceed 50 characters.");
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
            Type = _methodReferenceType,
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
            Type = _methodReferenceType,
            Accessibility = Method.MethodAccessibility.Public
        };

        Action act = method.ValidateFields;

        act.Should().Throw<ArgumentException>()
            .WithMessage("Id must be a valid non-empty GUID.");
    }

    [TestMethod]
    public void MethodValidator_WithValidDataType_ShouldNotThrow()
    {
        var method = new Method
        {
            Id = Guid.NewGuid(),
            Name = "mmmm",
            Type = _methodReferenceType,
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
            Type = _methodReferenceType
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
            Type = _methodReferenceType,
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
        var param = new ValueType("variable", "int", []);
        method.Parameters.Add(param);
        method.Parameters.Should().Contain(param);
    }

    [TestMethod]
    public void LocalVariables_AddLocalVariable_ShouldContainLocalVariable()
    {
        var method = new Method();
        var localVar = new ValueType("variable", "int", []);
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
            Type = _methodReferenceType,
            Accessibility = Method.MethodAccessibility.Public
        };

        var nameField = typeof(Method).GetField("_name", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        nameField!.SetValue(method, "1Invalid");

        Action act = method.ValidateFields;
        act.Should().Throw<ArgumentException>().WithMessage("Name cannot be null or start with a num.");
    }

    [TestMethod]
    public void ValidateFields_WithNullName_ShouldThrow()
    {
        var method = new Method
        {
            Id = Guid.NewGuid(),
            Type = _methodReferenceType,
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
        method.ClassId = classInstance.Id;
        method.ClassId.Should().Be(classInstance.Id);
    }

    [TestMethod]
    public void TypeId_SetAndGet_ShouldBeEqual()
    {
        var expectedTypeId = Guid.NewGuid();
        var method = new Method { TypeId = expectedTypeId };

        method.TypeId.Should().Be(expectedTypeId);
    }
}
