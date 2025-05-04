using FluentAssertions;

namespace ObjectSim.Domain.Test;

[TestClass]
public class ClassTest
{
    private Class? _testClass;
    private static readonly DataType ValueType = new ValueType("int", "int", []);
    private readonly Method _existingMethod = new()
    {
        Name = "ExistingMethod",
        Type = ValueType,
        Parameters = [],
        IsOverride = false
    };
    private readonly Attribute _attribute = new()
    {
        Name = "TestAttribute"
    };

    [TestInitialize]
    public void Initialize()
    {
        _testClass = new Class()
        {
            Name = "TestClass",
            IsAbstract = false,
            IsSealed = false,
            IsInterface = false,
            Attributes = [],
            Methods = [],
            Parent = null
        };;
    }

    #region CreateClass

    #region Error

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CreateClass_WithoutName_ThrowsException()
    {
        _testClass!.Name = null!;
    }

    [TestMethod]
    public void CreateClass_WithNameLongerThan20Characters_ThrowsException()
    {
        const string longName = "20CharactersLongNameeeeeeeeeee";

        Action action = () => _testClass!.Name = longName;

        action.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot be longer than 20 characters");
    }

    [TestMethod]
    public void CreateClass_WithNameShorterThan3Characters_ThrowsException()
    {
        const string shortName = "ab";

        Action action = () => _testClass!.Name = shortName;

        action.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot be shorter than 3 characters");
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CreateClass_SetAbstractionNull_ThrowsException()
    {
        _testClass!.IsAbstract = null;
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CreateClass_SetInterfaceNull_ThrowsException()
    {
        _testClass!.IsInterface = null;
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CreateClass_SetSealedNull_ThrowsException()
    {
        _testClass!.IsSealed = null;
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CreateClass_WithNullAttributes_ThrowsException()
    {
        _testClass!.Attributes = null;
    }

    [TestMethod]
    public void CreateClass_WithAttributesAndIsInterface_SetsEmptyList()
    {
        _testClass!.IsInterface = true;

        var attributes = new List<Attribute>
        {
            new Attribute
            {
                Name = "TestAttribute",
            }
        };

        _testClass.Attributes = attributes;

        _testClass.Attributes.Should().NotBeNull();
        _testClass.Attributes.Should().BeEmpty();
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CreateClass_WithNullMethods_ThrowsException()
    {
        _testClass!.Methods = null;
    }

    [TestMethod]
    public void CreateClass_WithNotAbstractMethodsAndIsInterface_ThrowsException()
    {
        _testClass!.IsInterface = true;

        var methods = new List<Method>
        {
            new Method
            {
                Name = "TestMethod",
                Abstract = false
            }
        };

        Action action = () => _testClass.Methods = methods;

        action.Should().Throw<ArgumentException>()
            .WithMessage("Methods in an interface must be abstract");
    }

    [TestMethod]
    public void CreateClass_WithSealedParent_ThrowsException()
    {
        var sealedParent = new Class() { IsSealed = true };

        Action action = () => _testClass!.Parent = sealedParent;

        action.Should().Throw<ArgumentException>()
            .WithMessage("Parent class cannot be sealed");
    }

    #endregion

    #region Success

    [TestMethod]
    public void CreateClass_WithValidName_SetValidName()
    {
        const string validName = "ValidName";

        Action action = () => _testClass!.Name = validName;
        action.Should().NotThrow();
        _testClass!.Name.Should().Be(validName);
    }

    [TestMethod]
    public void CreateClass_WithNotSealedParent_SetParent()
    {
        var validParent = new Class
        {
            Name = "validName",
            IsAbstract = false,
            IsSealed = false,
            IsInterface = false,
            Attributes = [],
            Methods = [],
            Parent = null
        };

        Action action = () => _testClass!.Parent = validParent;
        action.Should().NotThrow();
        _testClass!.Parent.Should().Be(validParent);
    }

    [TestMethod]
    public void CreateClass_ValidClass_SetValidName()
    {
        const string validName = "ValidName";
        var id = Guid.NewGuid();
        var attribute = new Attribute
        {
            Name = "TestAttribute",
        };
        var method = new Method
        {
            Name = "TestMethod",
        };

        var test = new Class
        {
            Name = validName,
            IsAbstract = false,
            IsSealed = false,
            Attributes = [attribute],
            Methods = [method],
            Parent = null,
            Id = id
        };
        test.Name.Should().Be(validName);
        test.IsAbstract.Should().BeFalse();
        test.IsSealed.Should().BeFalse();
        test.Attributes.Should().HaveCount(1);
        test.Methods.Should().HaveCount(1);
        test.Parent.Should().BeNull();
        test.Id.Should().Be(id);
    }

    #endregion

    #endregion

    #region CanAddMethod

    #region Error

    [TestMethod]
    public void CanAddMethod_WhenMethodAlreadyExistsInClass_ThrowsException()
    {
        _testClass!.Methods = [_existingMethod];

        var newMethod = new Method
        {
            Name = "ExistingMethod",
            Type = ValueType,
            Parameters = [],
            IsOverride = false
        };

        Action action = () => Class.CanAddMethod(_testClass, newMethod);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Method already exists in class.");
    }

    [TestMethod]
    public void CanAddMethod_TryingToAddRepeatedMethodNotOverriding_ThrowsException()
    {
        _testClass!.Methods!.Add(_existingMethod);

        var newMethod = new Method
        {
            Name = "ExistingMethod",
            Type = ValueType,
            Parameters = [],
            IsOverride = false
        };

        Action action = () => Class.CanAddMethod(_testClass, newMethod);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Method already exists in class.");
    }

    [TestMethod]
    public void CanAddMethod_ClassIsInterfaceMethodIsSealed_ThrowsException()
    {
        _testClass!.IsInterface = true;

        _existingMethod.IsSealed = true;

        Action action = () => Class.CanAddMethod(_testClass, _existingMethod);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Method cannot be sealed in an interface.");
    }

    [TestMethod]
    public void CanAddMethod_ClassIsInterfaceMethodIsOverriding_ThrowsException()
    {
        _testClass!.IsInterface = true;

        _existingMethod.IsOverride = true;

        Action action = () => Class.CanAddMethod(_testClass, _existingMethod);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Method cannot be override in an interface.");
    }

    [TestMethod]
    public void CanAddMethod_ClassIsInterfaceMethodAccessibilityIsPrivate_ThrowsException()
    {
        _testClass!.IsInterface = true;

       _existingMethod.Accessibility = Method.MethodAccessibility.Private;

        Action action = () => Class.CanAddMethod(_testClass, _existingMethod);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Method cannot be private in an interface.");
    }

    [TestMethod]
    public void CanAddMethod_ClassIsInterfaceMethodThatHaveLocalVariables_ThrowsException()
    {
        _testClass!.IsInterface = true;

        _existingMethod.LocalVariables = [ValueType];

        Action action = () => Class.CanAddMethod(_testClass, _existingMethod);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Method cannot have local variables in an interface.");
    }

    [TestMethod]
    public void CanAddMethod_ClassIsInterfaceMethodThatHaveMethodInvoke_ThrowsException()
    {
        _testClass!.IsInterface = true;

        _existingMethod.MethodsInvoke = [new Method()];

        Action action = () => Class.CanAddMethod(_testClass, _existingMethod);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Method cannot invoke other methods in an interface.");
    }

    [TestMethod]
    public void CanAddMethod_WithSameParameterCountAndAreSameParameters_ThrowsException()
    {
        var method = new Method
        {
            Name = "NewMethod",
            Type = ValueType,
            Parameters = [ValueType],
            IsOverride = false
        };

        _testClass!.Methods = [method];

        var newMethod = new Method
        {
            Name = "NewMethod",
            Type = ValueType,
            Parameters = [ValueType],
            IsOverride = false
        };

        Action action = () => Class.CanAddMethod(_testClass, newMethod);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Method already exists in class.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void CanAddMethod_WithCompletelyDifferentMethod_AddsMethods()
    {
        var method = new Method
        {
            Name = "NewMethod",
            Type = ValueType,
            Parameters = [],
            IsOverride = false
        };

        Action action = () => Class.CanAddMethod(_testClass!, method);

        action.Should().NotThrow();
    }

     [TestMethod]
    public void CanAddMethod_WithSameParamsInDifferentOrder_AddsMethod()
    {
        var parameterOne = new ValueType("int", "int", []);
        var parameterTwo = new ReferenceType("string", "string", []);

        var method = new Method
        {
            Name = "NewMethod",
            Type = ValueType,
            Parameters = [parameterOne, parameterTwo],
            IsOverride = false
        };

        _testClass!.Methods = [method];

        var newMethod = new Method
        {
            Name = "NewMethod",
            Type = ValueType,
            Parameters = [parameterTwo, parameterOne],
            IsOverride = false
        };

        Action action = () => Class.CanAddMethod(_testClass, newMethod);

        action.Should().NotThrow();
    }

    [TestMethod]
    public void CanAddMethod_WithSameNameAndTypeButDifferentParameters_AddsMethod()
    {
        var method = new Method
        {
            Name = "NewMethod",
            Type = ValueType,
            Parameters = [new ValueType("int", "int", [])],
            IsOverride = false
        };

        Action action = () => Class.CanAddMethod(_testClass!, method);

        action.Should().NotThrow();
    }

    [TestMethod]
    public void CanAddMethod_WithDifferentParameterCount_AddsMethod()
    {
        var method = new Method
        {
            Name = "NewMethod",
            Type = ValueType,
            Parameters = [new ValueType("int", "int", []), new ReferenceType("string", "string", [])],
            IsOverride = false
        };

        _testClass!.Methods = [method];

        var newMethod = new Method
        {
            Name = "NewMethod",
            Type = ValueType,
            Parameters = [new ValueType("int", "int", [])],
            IsOverride = false
        };

        Action action = () => Class.CanAddMethod(_testClass, newMethod);

        action.Should().NotThrow();
    }

    [TestMethod]
    public void CanAddMethod_TryingToAddOverridingParentMethod_AddsMethod()
    {
        var parentMethod = new Method
        {
            Name = "ParentMethod",
            Type = ValueType,
            Parameters = [],
            IsOverride = false
        };

        var parent = new Class { Methods = [parentMethod] };

        _testClass!.Parent = parent;

        var method = new Method
        {
            Name = "ParentMethod",
            Type = ValueType,
            Parameters = [],
            IsOverride = true
        };

        Action action = () => Class.CanAddMethod(_testClass, method);

        action.Should().NotThrow();
    }

    [TestMethod]
    public void CanAddMethod_ClassIsInterfaceMethodIsNotAbstract_MakeMethodAbstractAndAddsMethod()
    {
        _testClass!.IsInterface = true;

        var method = new Method
        {
            Name = "NewMethod",
            Type = ValueType,
            Parameters = [],
            IsOverride = false
        };

        Action action = () => Class.CanAddMethod(_testClass, method);

        action.Should().NotThrow();
        method.Abstract.Should().BeTrue();
    }

    [TestMethod]
    public void CanAddMethod_ClassIsInterfaceValidMethod_AddsMethod()
    {
        _testClass!.IsInterface = true;

        var method = new Method
        {
            Name = "NewMethod",
            Type = ValueType,
            Parameters = [],
            IsOverride = false,
            Abstract = true,
            IsSealed = false,
            Accessibility = Method.MethodAccessibility.Public,
            LocalVariables = [],
            MethodsInvoke = []
        };

        Action action = () => Class.CanAddMethod(_testClass, method);

        action.Should().NotThrow();
    }

    #endregion

    #endregion

    #region CanAddAttribute

    #region Error

    [TestMethod]
    public void CanAddAttribute_ClassIsInterface_ThrowsException()
    {
        _testClass!.IsInterface = true;

        Action action = () => Class.CanAddAttribute(_testClass, _attribute);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Cannot add attribute to an interface.");
    }

    [TestMethod]
    public void CanAddAttribute_AttributeRepeatedName_ThrowsException()
    {
        _testClass!.Attributes = [new Attribute { Name = "TestAttribute" }];

        Action action = () => Class.CanAddAttribute(_testClass, _attribute);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Attribute name already exists in class.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void CanAddAttribute_ValidAttribute_AddsAttribute()
    {

        Action action = () => Class.CanAddAttribute(_testClass!, _attribute);

        action.Should().NotThrow();
    }

    #endregion

    #endregion
}
