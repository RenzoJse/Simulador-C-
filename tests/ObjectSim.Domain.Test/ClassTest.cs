using FluentAssertions;

namespace ObjectSim.Domain.Test;

[TestClass]
public class ClassTest
{
    private Class? _testClass;
    private Method? _existingMethod;
    private Variable? _valueType;

    private static readonly Guid TypeId = Guid.NewGuid();

    private readonly Attribute _testAttribute = new() { Name = "TestAttribute" };

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
        };

        _existingMethod = new Method()
        {
            Name = "ExistingMethod",
            TypeId = TypeId,
            Parameters = [],
            IsOverride = false
        };

        _valueType = new Variable(Guid.NewGuid(), "int", _existingMethod);
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

        var attributes = new List<Attribute> { new Attribute { Name = "TestAttribute", } };

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

        var methods = new List<Method> { new Method { Name = "TestMethod", Abstract = false } };

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
        var attribute = new Attribute { Name = "TestAttribute", };
        var method = new Method { Name = "TestMethod", };

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
        _testClass!.Methods = [_existingMethod!];

        var newMethod = new Method { Name = "ExistingMethod", TypeId = TypeId, Parameters = [], IsOverride = false };

        Action action = () => _testClass!.CanAddMethod(newMethod);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Method already exists in class.");
    }

    [TestMethod]
    public void CanAddMethod_TryingToAddRepeatedMethodNotOverriding_ThrowsException()
    {
        _testClass!.Methods!.Add(_existingMethod!);

        var newMethod = new Method { Name = "ExistingMethod", TypeId = TypeId, Parameters = [], IsOverride = false };

        Action action = () => _testClass!.CanAddMethod(newMethod);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Method already exists in class.");
    }

    [TestMethod]
    public void CanAddMethod_ClassIsInterfaceMethodIsSealed_ThrowsException()
    {
        _testClass!.IsInterface = true;

        _existingMethod!.IsSealed = true;

        Action action = () => _testClass!.CanAddMethod(_existingMethod);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Method cannot be sealed in an interface.");
    }

    [TestMethod]
    public void CanAddMethod_ClassIsInterfaceMethodIsOverriding_ThrowsException()
    {
        _testClass!.IsInterface = true;

        _existingMethod!.IsOverride = true;

        Action action = () => _testClass!.CanAddMethod(_existingMethod);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Method cannot be override in an interface.");
    }

    [TestMethod]
    public void CanAddMethod_ClassIsInterfaceMethodAccessibilityIsPrivate_ThrowsException()
    {
        _testClass!.IsInterface = true;

        _existingMethod!.Accessibility = Method.MethodAccessibility.Private;

        Action action = () => _testClass!.CanAddMethod(_existingMethod);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Method cannot be private in an interface.");
    }

    [TestMethod]
    public void CanAddMethod_ClassIsInterfaceMethodThatHaveLocalVariables_ThrowsException()
    {
        _testClass!.IsInterface = true;

        _existingMethod!.LocalVariables = [_valueType!];

        Action action = () => _testClass!.CanAddMethod(_existingMethod);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Method cannot have local variables in an interface.");
    }

    [TestMethod]
    public void CanAddMethod_ClassIsInterfaceMethodThatHaveMethodInvoke_ThrowsException()
    {
        _testClass!.IsInterface = true;

        var methodToInvoke = new Method
        {
            Id = Guid.NewGuid(),
            Name = "MethodToInvoke",
            TypeId = TypeId,
            Abstract = true
        };

        _testClass.Methods = [methodToInvoke];

        var invokeMethod = new InvokeMethod(_existingMethod!.Id, methodToInvoke.Id, "this");

        _existingMethod.CanAddInvokeMethod(methodToInvoke, _testClass, "this");
        _existingMethod.MethodsInvoke.Add(invokeMethod);
        Action action = () => _testClass.CanAddMethod(_existingMethod);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Method cannot invoke other methods in an interface.");
    }

    [TestMethod]
    public void CanAddMethod_WithSameParameterCountAndAreSameParameters_ThrowsException()
    {
        var method = new Method { Name = "NewMethod", TypeId = TypeId, Parameters = [_valueType!], IsOverride = false };

        _testClass!.Methods = [method];

        var newMethod = new Method
        {
            Name = "NewMethod",
            TypeId = TypeId,
            Parameters = [_valueType!],
            IsOverride = false
        };

        Action action = () => _testClass!.CanAddMethod(newMethod);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Method already exists in class.");
    }

    [TestMethod]
    public void CanAddMethod_WhenOverrideWithoutVirtualParent_ShouldThrow()
    {
        var parentMethod = new Method { Name = "Test", IsVirtual = false };
        var parentClass = new Class { Methods = [parentMethod] };
        var childClass = new Class { Parent = parentClass, Methods = [] };

        var method = new Method { Name = "Test", IsOverride = true };

        Action act = () => childClass.CanAddMethod(method);

        act.Should().Throw<ArgumentException>()
            .WithMessage("Override method must override a method from the parent class.");
    }

    [TestMethod]
    public void CanAddMethod_WhenSameNameAndNotOverride_ShouldThrow()
    {
        var typeId = Guid.NewGuid();

        var method1 = new Method { Name = "TestMethod", TypeId = typeId, Parameters = [_valueType!], IsOverride = false };

        var method2 = new Method { Name = "TestMethod", TypeId = typeId, Parameters = [_valueType!], IsOverride = false };

        var classObj = new Class { Methods = [method1] };

        Action act = () => classObj.CanAddMethod(method2);

        act.Should().Throw<ArgumentException>()
            .WithMessage("Method already exists in class.");
    }

    [TestMethod]
    public void CanAddMethod_WhenMethodIsStaticAndClassIsInterface_ShouldThrow()
    {
        _testClass!.IsInterface = true;

        var method = new Method
        {
            Name = "StaticMethod",
            TypeId = TypeId,
            Parameters = [],
            IsOverride = false,
            IsStatic = true
        };

        Action action = () => _testClass!.CanAddMethod(method);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Method cannot be static in an interface.");
    }

    [TestMethod]
    public void CanAddMethod_WhenMethodIsStaticAndClassIsAbstract_ShouldThrow()
    {
        _testClass!.IsAbstract = true;

        var method = new Method
        {
            Name = "StaticMethod",
            TypeId = TypeId,
            Parameters = [],
            IsOverride = false,
            IsStatic = true
        };

        Action action = () => _testClass!.CanAddMethod(method);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Method cannot be static in an Abstract Class.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void CanAddMethod_WhenOverrideWithVirtualParent_ShouldSucceed()
    {
        var parentMethod = new Method { Name = "Test", IsVirtual = true };
        var parentClass = new Class { Methods = [parentMethod] };
        var childClass = new Class { Parent = parentClass, Methods = [] };

        var method = new Method { Name = "Test", IsOverride = true };

        Action act = () => childClass.CanAddMethod(method);

        act.Should().NotThrow();
    }

    [TestMethod]
    public void CanAddMethod_WithCompletelyDifferentMethod_AddsMethods()
    {
        var method = new Method
        {
            Name = "NewMethod",
            TypeId = TypeId,
            Parameters = [],
            IsOverride = false
        };

        Action action = () => _testClass!.CanAddMethod(method);

        action.Should().NotThrow();
    }

    [TestMethod]
    public void CanAddMethod_WithSameParamsInDifferentOrder_AddsMethod()
    {
        var method = new Method
        {
            Name = "NewMethod",
            TypeId = TypeId,
            Parameters = [],
            IsOverride = false
        };

        var parameterOne = new Variable(Guid.NewGuid(), "int", method);
        var parameterTwo = new Variable(Guid.NewGuid(), "string", method);
        method.Parameters = [parameterOne, parameterTwo];

        _testClass!.Methods = [method];

        var newMethod = new Method
        {
            Name = "NewMethod",
            TypeId = TypeId,
            Parameters = [parameterTwo, parameterOne],
            IsOverride = false
        };

        Action action = () => _testClass!.CanAddMethod(newMethod);

        action.Should().NotThrow();
    }

    [TestMethod]
    public void CanAddMethod_WithSameNameAndTypeButDifferentParameters_AddsMethod()
    {
        var method = new Method
        {
            Name = "NewMethod",
            TypeId = TypeId,
            Parameters = [_valueType!],
            IsOverride = false
        };

        Action action = () => _testClass!.CanAddMethod(method);

        action.Should().NotThrow();
    }

    [TestMethod]
    public void CanAddMethod_WithDifferentParameterCount_AddsMethod()
    {
        var method = new Method
        {
            Name = "NewMethod",
            TypeId = TypeId,
            Parameters = [_valueType!],
            IsOverride = false
        };

        _testClass!.Methods = [method];

        var newMethod = new Method
        {
            Name = "NewMethod",
            TypeId = TypeId,
            Parameters = [_valueType!, _valueType!],
            IsOverride = false
        };

        Action action = () => _testClass!.CanAddMethod(newMethod);

        action.Should().NotThrow();
    }

    [TestMethod]
    public void CanAddMethod_TryingToAddOverridingParentMethod_AddsMethod()
    {
        var typeId = Guid.NewGuid();

        var parentMethod = new Method
        {
            Name = "DoWork",
            TypeId = typeId,
            Parameters = [_valueType!],
            IsVirtual = true,
            IsOverride = false
        };

        var childOverride = new Method
        {
            Name = "DoWork",
            TypeId = typeId,
            Parameters = [_valueType!],
            IsOverride = true,
            IsVirtual = false
        };

        var parentClass = new Class
        {
            Methods = [parentMethod]
        };

        var childClass = new Class
        {
            Parent = parentClass,
            Methods = []
        };

        Action act = () => childClass.CanAddMethod(childOverride);

        act.Should().NotThrow();
    }

    [TestMethod]
    public void CanAddMethod_ClassIsInterfaceMethodIsNotAbstract_MakeMethodAbstractAndAddsMethod()
    {
        _testClass!.IsInterface = true;

        var method = new Method
        {
            Name = "NewMethod",
            TypeId = TypeId,
            Parameters = [],
            IsOverride = false
        };

        Action action = () => _testClass!.CanAddMethod(method);

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
            TypeId = TypeId,
            Parameters = [],
            IsOverride = false,
            Abstract = true,
            IsSealed = false,
            Accessibility = Method.MethodAccessibility.Public,
            LocalVariables = [],
            MethodsInvoke = []
        };

        Action action = () => _testClass!.CanAddMethod(method);

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

        Action action = () => _testClass.CanAddAttribute(_testAttribute);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Cannot add attribute to an interface.");
    }

    [TestMethod]
    public void CanAddAttribute_AttributeRepeatedName_ThrowsException()
    {
        _testClass!.Attributes = [new Attribute { Name = "TestAttribute" }];

        Action action = () => _testClass.CanAddAttribute(_testAttribute);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Attribute name already exists in class.");
    }

    [TestMethod]
    public void AddAttribute_ShouldThrow_WhenDuplicateName()
    {
        var attribute2 = new Attribute { Name = _testAttribute.Name };
        _testClass!.AddAttribute(_testAttribute);

        var act = () => _testClass.AddAttribute(attribute2);

        act.Should().Throw<ArgumentException>()
            .WithMessage("Attribute name already exists in class.");
    }

    [TestMethod]
    public void AddAttribute_ShouldThrow_WhenClassIsInterface()
    {
        _testClass!.IsInterface = true;

        var act = () => _testClass.AddAttribute(_testAttribute);

        act.Should().Throw<ArgumentException>().WithMessage("Cannot add attribute to an interface.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void CanAddAttribute_ValidAttribute_AddsAttribute()
    {
        Action action = () => _testClass!.CanAddAttribute(_testAttribute);

        action.Should().NotThrow();
    }

    [TestMethod]
    public void AddAttribute_ShouldAddAttribute_WhenValid()
    {
        _testClass!.AddAttribute(_testAttribute);

        _testClass.Attributes.Should().ContainSingle(a => a.Name == _testAttribute.Name);
    }

    #endregion

    #endregion
}
