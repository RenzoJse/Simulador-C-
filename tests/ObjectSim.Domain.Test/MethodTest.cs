using FluentAssertions;

namespace ObjectSim.Domain.Test;

[TestClass]
public class MethodTest
{
    private readonly DataType _methodType = new ValueType("methodType", "int", []);
    private readonly DataType _methodReferenceType = new ReferenceType("methodReferenceType", "object", []);

    private Class? _testClass = new Class
    {
        Id = Guid.NewGuid(),
        Name = "TestClass",
        Methods = []
    };

    private Method? _testMethod;

    [TestInitialize]
    public void Initialize()
    {
        _testMethod =  new Method
        {
            Id = Guid.NewGuid(),
            Name = "TestMethod",
            Type = new ValueType("int", "int", []),
            Accessibility = Method.MethodAccessibility.Public,
            Abstract = false,
            IsSealed = false,
            IsOverride = false,
            ClassId = _testClass!.Id,
            Class = _testClass
        };
    }

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
        method.Class = classInstance;
        method.ClassId = classInstance.Id;

        method.Class.Should().Be(classInstance);
        method.ClassId.Should().Be(classInstance.Id);
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
        var parentClass = new Class { Methods = [] };
        _testClass!.Parent = parentClass;

        var otherMethod = new Method { Id = Guid.NewGuid() };
        _testClass.Methods!.Add(new Method { Id = Guid.NewGuid() });

        Action act = () => _testMethod!.MethodsInvoke = [otherMethod];

        act.Should().Throw<ArgumentException>()
            .WithMessage("The invoked method must be reachable from the current method.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void SetInvokeMethod_WhenUsingMethodThatIsNotInClassButInParentClass_AddsMethod()
    {
        var parentClass = new Class
        {
            Id = Guid.NewGuid(),
            Name = "ParentClass",
            Methods = []
        };
        _testClass!.Parent = parentClass;

        var otherMethod = new Method
        {
            Id = Guid.NewGuid(),
            Name = "OtherMethod",
            Class = parentClass
        };
        parentClass.Methods!.Add(otherMethod);

        _testMethod!.MethodsInvoke = [otherMethod];
        _testMethod.MethodsInvoke.Should().Contain(otherMethod);
    }

    #endregion

    #endregion
}
