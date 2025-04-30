using FluentAssertions;

namespace ObjectSim.Domain.Test;

[TestClass]
public class MethodTest
{
    private Method? _testMethod;

    private Class? _testClass = new Class
    {
        Id = Guid.NewGuid(),
        Name = "TestClass",
    };

    [TestInitialize]
    public void Initialize()
    {
        _testMethod = new Method();
    }

    [TestMethod]
    public void DataType_Property_SetAndGet_ShouldBeEqual()
    {
        _testMethod!.Type = Method.MethodDataType.Decimal;
        _testMethod.Type.Should().Be(Method.MethodDataType.Decimal);
    }

    [TestMethod]
    public void Accessibility_Property_SetAndGet_ShouldBeEqual()
    {
        _testMethod!.Accessibility = Method.MethodAccessibility.ProtectedInternal;
        _testMethod.Accessibility.Should().Be(Method.MethodAccessibility.ProtectedInternal);
    }

    [TestMethod]
    public void MethodDataType_CreateMethod_ShouldSetCorrectly()
    {
        _testMethod!.Type = Method.MethodDataType.String;
        Assert.AreEqual(Method.MethodDataType.String, _testMethod.Type);
    }

    [TestMethod]
    public void MethodAccessibility_CreateMethod_ShouldSetCorrectly()
    {
        _testMethod!.Accessibility = Method.MethodAccessibility.Public;
        Assert.AreEqual(Method.MethodAccessibility.Public, _testMethod.Accessibility);
    }

    [TestMethod]
    public void Name_Property_SetAndGet_ShouldBeEqual()
    {
        _testMethod!.Name = "TestMethod";
        _testMethod.Name.Should().Be("TestMethod");
    }

    [TestMethod]
    public void Id_Property_SetAndGet_ShouldBeEqual()
    {
        var id = Guid.NewGuid();
        _testMethod!.Id = id;
        _testMethod.Id.Should().Be(id);
    }

    [TestMethod]
    public void Name_SetToEmpty_ShouldThrowArgumentException()
    {
        Action act = () => _testMethod!.Name = string.Empty;

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

    #region InvokeMethod

    #region Error

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void SetInvokeMethod_WhenMethodsInvokeAreNull_ThrowsNullException()
    {
        var method = new Method { MethodsInvoke = null! };
    }

    [TestMethod]
    public void SetInvokeMethod_WhenOtherMethodIsNotInClass_ThrowsException()
    {
        var otherMethod = new Method { Id = Guid.NewGuid() };
        _testMethod!.Class = _testClass;

        Action act = () => _testMethod!.MethodsInvoke = [otherMethod];

        act.Should().Throw<ArgumentException>()
            .WithMessage("The invoked method must be reachable from the current method.");
    }

    [TestMethod]
    public void SetInvokeMethod_WhenIsTryingToUseWrongAttributeMethod_ThrowsException()
    {
        // nose puede hacer aun
    }

    [TestMethod]
    public void SetInvokeMethod_WhenUsingMethodThatIsNotInClassNeitherParentClass_ThrowsException()
    {
        var otherMethod = new Method { Id = Guid.NewGuid() };
        _testMethod!.Class = _testClass;

        Action act = () => _testMethod!.MethodsInvoke = [otherMethod];

        act.Should().Throw<ArgumentException>()
            .WithMessage("The invoked method must be reachable from the current method.");
    }

    #endregion

    #endregion
}
