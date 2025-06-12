using FluentAssertions;

namespace ObjectSim.Domain.Test;

[TestClass]
public class MethodTest
{
    private readonly DataType _methodType = new ValueType(Guid.NewGuid(), "int");
    private readonly DataType _methodReferenceType = new ReferenceType(Guid.NewGuid(), "object");

    private readonly Class? _testClass = new Class
    {
        Id = Guid.NewGuid(),
        Name = "TestClass",
        Methods = [],
        Attributes = [],
    };

    private Method? _testMethod;

    private static readonly Method? OtherMethod = new Method
    {
        Id = Guid.NewGuid(),
        Name = "OtherMethod",
        TypeId = Guid.NewGuid()
    };

    private readonly InvokeMethod _testInvokeMethod = new InvokeMethod(OtherMethod!.Id, Guid.NewGuid(), "this");

    [TestInitialize]
    public void Initialize()
    {
        _testMethod = new Method
        {
            Id = Guid.NewGuid(),
            Name = "TestMethod",
            TypeId = Guid.NewGuid(),
            Accessibility = Method.MethodAccessibility.Public,
            Abstract = false,
            IsSealed = false,
            IsOverride = false,
            ClassId = _testClass!.Id
        };
    }

    #region DataType

    [TestMethod]
    public void DataType_Property_SetAndGet_ShouldBeEqual()
    {
        var method = new Method { TypeId = _methodType.Id };
        method.TypeId.Should().Be(_methodType.Id);
    }

    [TestMethod]
    public void MethodDataType_CreateMethod_ShouldSetCorrectly()
    {
        var method = new Method { TypeId = _methodReferenceType.Id };
        Assert.AreEqual(_methodReferenceType.Id, method.TypeId);
    }

    [TestMethod]
    public void TypeId_SetAndGet_ShouldBeEqual()
    {
        var expectedTypeId = Guid.NewGuid();
        var method = new Method { TypeId = expectedTypeId };
        method.TypeId.Should().Be(expectedTypeId);
    }

    #endregion

    #region Accessibility

    [TestMethod]
    public void Accessibility_Property_SetAndGet_ShouldBeEqual()
    {
        var method = new Method { Accessibility = Method.MethodAccessibility.Public };
        method.Accessibility.Should().Be(Method.MethodAccessibility.Public);
    }

    [TestMethod]
    public void MethodAccessibility_CreateMethod_ShouldSetCorrectly()
    {
        _testMethod!.Accessibility = Method.MethodAccessibility.Public;
        Assert.AreEqual(Method.MethodAccessibility.Public, _testMethod.Accessibility);
    }

    #endregion

    #region Name

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

    #endregion

    #region Validate

    [TestMethod]
    public void Validate_ValidMethod_ShouldNotThrow()
    {
        var method = new Method
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            TypeId = _methodReferenceType.Id,
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
            TypeId = _methodReferenceType.Id,
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
            TypeId = _methodReferenceType.Id,
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
            TypeId = _methodReferenceType.Id
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
            TypeId = _methodReferenceType.Id,
            Accessibility = Method.MethodAccessibility.Public
        };
        Action act = method.ValidateFields;
        act.Should().NotThrow();
    }

    [TestMethod]
    public void ValidateFields_WithInvalidName_ShouldThrow()
    {
        var method = new Method
        {
            Id = Guid.NewGuid(),
            TypeId = _methodReferenceType.Id,
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
            TypeId = _methodReferenceType.Id,
            Accessibility = Method.MethodAccessibility.Public
        };
        var nameField = typeof(Method).GetField("_name", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        nameField!.SetValue(method, null);
        Action act = method.ValidateFields;
        act.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot be null or whitespace.");
    }

    #endregion

    #region Flags

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

    #endregion

    #region ParametersAndVariables

    [TestMethod]
    public void Parameters_AddParameter_ShouldContainParameter()
    {
        var method = new Method();
        var param = new Variable(Guid.NewGuid(), "int");
        method.Parameters.Add(param);
        method.Parameters.Should().Contain(param);
    }

    [TestMethod]
    public void LocalVariables_AddLocalVariable_ShouldContainLocalVariable()
    {
        var method = new Method();
        var localVar = new Variable(Guid.NewGuid(), "int");
        method.LocalVariables.Add(localVar);
        method.LocalVariables.Should().Contain(localVar);
    }

    #endregion

    #region Constructor

    [TestMethod]
    public void Id_DefaultValue_ShouldNotBeEmpty()
    {
        var method = new Method();
        method.Id.Should().NotBe(Guid.Empty);
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

    #endregion

    #region InvokeMethod

    #region Error

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddInvokeMethod_WhenMethodsInvokeAreNull_ThrowsNullException()
    {
        _testMethod!.CanAddInvokeMethod(null!, _testClass!, "this");
    }


    [TestMethod]
    public void AddInvokeMethod_WhenOtherMethodIsNotInClass_ThrowsException()
    {
        Action act = () => _testMethod!.CanAddInvokeMethod(OtherMethod!, _testClass!, "this");

        act.Should().Throw<ArgumentException>()
            .WithMessage("The invoked method must be reachable from the current method.");
    }

    [TestMethod]
    public void AddInvokeMethod_WhenIsTryingToUseWrongAttributeMethod_ThrowsException()
    {
        var attribute = new Attribute
        {
            Id = Guid.NewGuid(),
            DataType = new ValueType(Guid.NewGuid(), "int"),
        };
        _testClass!.Attributes = [attribute];

        Action act = () => _testMethod!.CanAddInvokeMethod(OtherMethod!, _testClass!, "this");

        act.Should().Throw<ArgumentException>()
            .WithMessage("The invoked method must be reachable from the current method.");
    }

    [TestMethod]
    public void AddInvokeMethod_WhenUsingMethodThatIsNotInClassNeitherParentClass_ThrowsException()
    {
        var unreachableMethod = new Method { Name = "GhostMethod" };

        var parentClass = new Class { Methods = [] };
        _testClass!.Parent = parentClass;
        _testClass.Methods!.Add(new Method { Name = "UnrelatedMethod" });

        Action act = () => _testMethod!.CanAddInvokeMethod(unreachableMethod, _testClass!, "this");

        act.Should().Throw<ArgumentException>()
            .WithMessage("The invoked method must be reachable from the current method.");
    }

    [TestMethod]
    public void AddInvokeMethod_WhenIsTryingToUseMethodNotInLocalVariables_ThrowsException()
    {
        var localVariable = new Variable(Guid.NewGuid(), "int");

        OtherMethod!.LocalVariables = [localVariable];

        Action act = () => _testMethod!.CanAddInvokeMethod(OtherMethod, _testClass!, "this");

        act.Should().Throw<ArgumentException>()
            .WithMessage("The invoked method must be reachable from the current method.");
    }

    [TestMethod]
    public void AddInvokeMethod_WhenIsTryingToUseMethodNotInParameters_ThrowsException()
    {
        var parameter = new Variable(Guid.NewGuid(), "int");

        OtherMethod!.Parameters = [parameter];

        Action act = () => _testMethod!.CanAddInvokeMethod(OtherMethod, _testClass!, "this");

        act.Should().Throw<ArgumentException>()
            .WithMessage("The invoked method must be reachable from the current method.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void AddInvokeMethod_WhenUsingMethodThatIsNotInClassButInParentClass_AddsMethod()
    {
        var parentClass = new Class
        {
            Id = Guid.NewGuid(),
            Name = "ParentClass",
            Methods = []
        };
        _testClass!.Parent = parentClass;

        parentClass.Methods!.Add(OtherMethod!);

        _testMethod!.CanAddInvokeMethod(OtherMethod!, _testClass!, "this");
        _testMethod.MethodsInvoke.Add(_testInvokeMethod);
        _testMethod.MethodsInvoke.Should().Contain(_testInvokeMethod!);
    }

    [TestMethod]
    public void AddInvokeMethod_WhenMethodIsInLocalVariable_AddsMethod()
    {
        var localVariable = new Variable(Guid.NewGuid(), "int");

        OtherMethod!.LocalVariables = [localVariable];

        _testMethod!.CanAddInvokeMethod(OtherMethod, _testClass!, "test");
        _testMethod.MethodsInvoke.Add(_testInvokeMethod);
        _testMethod.MethodsInvoke.Should().Contain(_testInvokeMethod);
    }

    #endregion

    #endregion
}
