using FluentAssertions;
using Moq;
using ObjectSim.ClassLogic.ClassBuilders;
using ObjectSim.ClassLogic.ClassBuilders.Builders;
using ObjectSim.ClassLogic.Strategy;
using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;
using Attribute = ObjectSim.Domain.Attribute;
using ValueType = ObjectSim.Domain.ValueType;

namespace ObjectSim.BusinessLogic.Test;

[TestClass]
public class ClassServiceTest
{
    private ClassService? _classServiceTest;
    private Mock<IBuilderStrategy>? _builderStrategyMock;
    private Mock<IAttributeService>? _attributeServiceMock;
    private Mock<IMethodServiceCreate>? _methodServiceCreateMock;
    private Mock<IMethodService>? _methodServiceMock;
    private Mock<IRepository<Class>>? _classRepositoryMock;

    private readonly Class _testInterfaceClass = new Class
    {
        Name = "TestClass",
        IsAbstract = false,
        IsInterface = true,
        IsSealed = false,
        Attributes = [],
        Methods = [],
        Parent = null,
    };

    private readonly Class _testClass = new Class
    {
        Name = "TestClass",
        IsAbstract = false,
        IsInterface = false,
        IsSealed = false,
        Attributes = [],
        Methods = [],
        Parent = null,
    };

    private readonly Method _testMethod = new Method
    {
        Name = "TestMethod",
    };

    private readonly Attribute _testAttribute = new Attribute
    {
        Name = "TestAttribute",
    };

    private readonly CreateClassArgs _args = new CreateClassArgs("TestClass",
        false,
        false,
        false,
        [],
        [],
        Guid.NewGuid());

    protected virtual Builder GetMockedBuilder()
    {
        _methodServiceMock = new Mock<IMethodService>();
        _methodServiceCreateMock = new Mock<IMethodServiceCreate>();
        _attributeServiceMock = new Mock<IAttributeService>();

        return new ClassBuilder(
            _methodServiceCreateMock.Object,
            _attributeServiceMock.Object);
    }

    [TestInitialize]
    public void Initialize()
    {
        _methodServiceMock = new Mock<IMethodService>(MockBehavior.Strict);
        _attributeServiceMock = new Mock<IAttributeService>(MockBehavior.Strict);
        _builderStrategyMock = new Mock<IBuilderStrategy>(MockBehavior.Strict);
        _classRepositoryMock = new Mock<IRepository<Class>>(MockBehavior.Strict);

        var strategies = new List<IBuilderStrategy> { _builderStrategyMock!.Object };

        _classServiceTest = new ClassService(strategies, _classRepositoryMock.Object);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _methodServiceMock?.VerifyAll();
        _attributeServiceMock?.VerifyAll();
        _builderStrategyMock?.VerifyAll();
        _classRepositoryMock?.VerifyAll();
    }

    #region CreateClass

    #region Error

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CreateClass_WithNullArgs_ThrowsException()
    {
        _classServiceTest!.CreateClass(null!);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CreateClass_WithEmptyName_ThrowsException()
    {
        var classBuilder = GetMockedBuilder();

        _builderStrategyMock!.Setup(x => x.WhichIsMyBuilder(It.IsAny<CreateClassArgs>())).Returns(true);
        _builderStrategyMock.Setup(x => x.CreateBuilder()).Returns(classBuilder);

        _args.Name = null;
        _classServiceTest!.CreateClass(_args);
    }

    [TestMethod]
    [ExpectedException(typeof(NullReferenceException))]
    public void CreateClass_WithNoMatchingStrategy_ThrowsException()
    {
        _builderStrategyMock!.Setup(x => x.WhichIsMyBuilder(It.IsAny<CreateClassArgs>())).Returns(false);

        _classServiceTest!.CreateClass(_args);
    }


    [TestMethod]
    public void CreateClass_WithNullAttributesList_ThrowsArgumentException()
    {
        var argsWithNullAttributes = new CreateClassArgs(
            "TestClass",
            false,
            false,
            false,
            null!,
            [],
            Guid.NewGuid());

        var classBuilder = GetMockedBuilder();
        _builderStrategyMock!.Setup(x => x.WhichIsMyBuilder(It.IsAny<CreateClassArgs>())).Returns(true);
        _builderStrategyMock.Setup(x => x.CreateBuilder()).Returns(classBuilder);

        Action action = () => _classServiceTest!.CreateClass(argsWithNullAttributes);

        action.Should().Throw<ArgumentException>();
    }

    #endregion

    #region Success

    [TestMethod]
    public void CreateClass_WithValidParameters_SetsStrategyBuilder()
    {
        var classBuilder = GetMockedBuilder();

        _builderStrategyMock!.Setup(x => x.WhichIsMyBuilder(_args)).Returns(true);
        _builderStrategyMock.Setup(x => x.CreateBuilder()).Returns(classBuilder);
        _classRepositoryMock!.Setup(repo => repo.Add(It.IsAny<Class>())).Returns((Class c) => c);

        Action action = () => _classServiceTest!.CreateClass(_args);

        action.Should().NotThrow();
    }

    [TestMethod]
    public void CreateClass_WithNullParent_LeavesNullParent()
    {
        _args.Parent = null;
        var classBuilder = GetMockedBuilder();

        _builderStrategyMock!.Setup(x => x.WhichIsMyBuilder(It.IsAny<CreateClassArgs>())).Returns(true);
        _builderStrategyMock.Setup(x => x.CreateBuilder()).Returns(classBuilder);
        _classRepositoryMock!.Setup(repo => repo.Add(It.IsAny<Class>())).Returns((Class c) => c).Verifiable();

        Action action = () => _classServiceTest!.CreateClass(_args);

        action.Should().NotThrow();
    }

    [TestMethod]
    public void CreateClass_WithValidParameters_ReturnsClass()
    {
        _args.Parent = null;
        _args.Attributes = [];
        _args.Methods = [];
        _args.Name = "TestClass";
        _args.IsAbstract = false;
        _args.IsInterface = false;
        _args.IsSealed = false;

        var classBuilder = GetMockedBuilder();

        _builderStrategyMock!.Setup(x => x.WhichIsMyBuilder(It.IsAny<CreateClassArgs>())).Returns(true);
        _builderStrategyMock.Setup(x => x.CreateBuilder()).Returns(classBuilder);
        _classRepositoryMock!.Setup(repo => repo.Add(It.IsAny<Class>())).Returns((Class c) => c).Verifiable();

        var result = _classServiceTest!.CreateClass(_args);
        result.Should().NotBeNull();
        result.Should().BeOfType<Class>();
        result.Name.Should().Be(_args.Name);
        result.IsAbstract.Should().Be(_args.IsAbstract);
        result.IsInterface.Should().Be(_args.IsInterface);
        result.IsSealed.Should().Be(_args.IsSealed);
        result.Attributes.Should().BeEquivalentTo(_args.Attributes);
        result.Methods.Should().BeEquivalentTo(_args.Methods);
        result.Parent.Should().Be(null);
        _classRepositoryMock.Verify(repo => repo.Add(It.IsAny<Class>()), Times.Once);
    }

    #endregion

    #endregion

    #region GetById

    #region Error

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void GetById_WithNullId_ThrowsException()
    {
        _classServiceTest!.GetById(null!);
    }

    [TestMethod]
    public void GetById_WithInexistantId_ThrowsException()
    {
        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns((Class?)null);

        Action action = () => _classServiceTest!.GetById(Guid.NewGuid());
        action.Should().Throw<ArgumentException>();
    }

    #endregion

    #region Success

    [TestMethod]
    public void GetById_WithValidId_ReturnsClass()
    {
        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(_testClass);

        var result = _classServiceTest!.GetById(_testClass.Id);
        result.Should().NotBeNull();
        result.Should().BeOfType<Class>();
        result.Should().Be(_testClass);
    }

    #endregion

    #endregion

    #region AddMethod

    #region Error

    [TestMethod]
    public void AddMethod_WithNullClassId_ThrowsException()
    {
        Action action = () => _classServiceTest!.AddMethod(null!, new Method());
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void AddMethod_WithNullMethod_ThrowsException()
    {
        Action action = () => _classServiceTest!.AddMethod(Guid.NewGuid(), null!);
        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void AddMethod_TryingToAddRepeatedMethodNotOverriding_ThrowsException()
    {
        _testClass.Methods!.Add(_testMethod);

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(_testClass);

        Action action = () => _classServiceTest!.AddMethod(_testClass.Id, _testMethod);
        action.Should().Throw<ArgumentException>("Method already exists in class.");
    }

    [TestMethod]
    public void AddMethod_ClassIsInterfaceMethodIsSealed_ThrowsException()
    {
        _testMethod.IsSealed = true;

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(_testInterfaceClass);

        Action action = () => _classServiceTest!.AddMethod(_testInterfaceClass.Id, _testMethod);
        action.Should().Throw<ArgumentException>("Method cannot be sealed in an interface.");
    }

    [TestMethod]
    public void AddMethod_ClassIsInterfaceMethodIsOverriding_ThrowsException()
    {
        _testMethod.IsOverride = true;

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(_testInterfaceClass);

        Action action = () => _classServiceTest!.AddMethod(_testInterfaceClass.Id, _testMethod);
        action.Should().Throw<ArgumentException>("Method cannot be overridden in an interface.");
    }

    [TestMethod]
    public void AddMethod_ClassIsInterfaceMethodAccesibilityIsPrivate_ThrowsException()
    {
        _testMethod.Accessibility = Method.MethodAccessibility.Private;

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(_testInterfaceClass);

        Action action = () => _classServiceTest!.AddMethod(_testInterfaceClass.Id, _testMethod);
        action.Should().Throw<ArgumentException>("Method cannot be private in an interface.");
    }

    [TestMethod]
    public void AddMethod_ClassIsInterfaceMethodThatHaveLocalVariables_ThrowsException()
    {
        _testMethod.LocalVariables = [new ValueType("variable", "int", [])];

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(_testInterfaceClass);

        Action action = () => _classServiceTest!.AddMethod(_testInterfaceClass.Id, _testMethod);
        action.Should().Throw<ArgumentException>("Method cannot be implemented in an interface.");
    }

    [TestMethod]
    public void AddMethod_ClassIsInterfaceMethodThatHaveMethodInvoke_ThrowsException()
    {
        _testMethod.MethodsInvoke = [new Method()];

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(_testInterfaceClass);

        Action action = () => _classServiceTest!.AddMethod(_testInterfaceClass.Id, _testMethod);
        action.Should().Throw<ArgumentException>("Method cannot be implemented in an interface.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void AddMethod_WithCompletelyDifferentMethod_AddsMethods()
    {
        var classId = Guid.NewGuid();

        var existingMethod = new Method
        {
            Name = "TestMethod1",
            Parameters = [new ValueType("variable", "int", [])]
        };

        var newMethod = new Method
        {
            Name = "TestMethod2",
            Parameters = [new ValueType("variable", "int", [])]
        };

        _testClass.Methods!.Add(existingMethod);

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(_testClass);

        _classServiceTest!.AddMethod(classId, newMethod);

        _testClass.Methods.Should().Contain(newMethod);
    }

    [TestMethod]
    public void AddMethod_WithSameParamsInDifferentOrder_AddsMethod()
    {
        var classId = Guid.NewGuid();

        var existingMethod = new Method
        {
            Name = "TestMethod",
            Parameters = [
                new ValueType("variable", "int", []),
                new ValueType("variableTwo", "bool", [])
            ]
        };

        var newMethod = new Method
        {
            Name = "TestMethod",
            Parameters = [
                new ValueType("variableTwo", "bool", []),
                new ValueType("variable", "int", [])
            ]
        };

        _testClass.Methods!.Add(existingMethod);

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(_testClass);

        _classServiceTest!.AddMethod(classId, newMethod);

        _testClass.Methods.Should().Contain(newMethod);
    }

    [TestMethod]
    public void AddMethod_WithSameNameAndTypeButDifferentParameters_AddsMethod()
    {
        var classId = Guid.NewGuid();

        var existingMethod = new Method
        {
            Name = "TestMethod",
            Abstract = false,
            IsSealed = false,
            Parameters = [new ValueType("variable", "bool", [])]
        };

        var newMethod = new Method
        {
            Name = "TestMethod",
            Abstract = false,
            IsSealed = false,
            Parameters = [new ValueType("variable", "int", [])]
        };

        _testClass.Methods!.Add(existingMethod);

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(_testClass);

        _classServiceTest!.AddMethod(classId, newMethod);

        _testClass.Methods.Should().Contain(newMethod);
    }

    [TestMethod]
    public void AddMethod_WithDifferentParameterCount_AddsMethod()
    {
        var classId = Guid.NewGuid();

        var existingMethod = new Method
        {
            Name = "TestMethod",
            Parameters = [new ValueType("variable", "bool", [])]
        };

        var newMethod = new Method
        {
            Name = "TestMethod",
            Parameters = [
                new ValueType("variable", "bool", []),
                new ValueType("variableTwo", "int", [])
            ]
        };

        var testClass = new Class
        {
            Id = classId,
            Name = "TestClass",
            Methods = [existingMethod]
        };

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(testClass);

        _classServiceTest!.AddMethod(classId, newMethod);

        testClass.Methods.Should().Contain(newMethod);
    }

    [TestMethod]
    public void AddMethod_TryingToAddOverridingParentMethod_AddsMethod()
    {
        var classId = Guid.NewGuid();

        var existingMethod = new Method
        {
            Name = "TestMethod",
            Parameters = [new ValueType("variable", "int", [])]
        };

        var newMethod = new Method
        {
            Name = "TestMethod",
            Parameters = [
                new ValueType("variable", "int", []),
            ],
            IsOverride = true
        };

        var testClass = new Class
        {
            Id = classId,
            Name = "TestClass",
            Methods = [existingMethod]
        };

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(testClass);

        _classServiceTest!.AddMethod(classId, newMethod);

        testClass.Methods.Should().Contain(newMethod);
    }

    [TestMethod]
    public void AddMethod_ClassIsInterfaceMethodIsNotAbstract_MakeMethodAbstractAndAddsMethod()
    {
        _testMethod.Abstract = false;

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(_testInterfaceClass);

        _classServiceTest!.AddMethod(_testInterfaceClass.Id, _testMethod);

        _testInterfaceClass.Methods.Should().Contain(_testMethod);
    }

    [TestMethod]
    public void AddMethod_ClassIsInterfaceValidMethod_AddsMethod()
    {
        var validMethod = new Method
        {
            Name = "TestMethod",
            Abstract = true,
            IsSealed = false,
            Parameters = [],
        };

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(_testInterfaceClass);

        _classServiceTest!.AddMethod(_testInterfaceClass.Id, validMethod);

        _testInterfaceClass.Methods.Should().Contain(validMethod);
    }

    #endregion

    #endregion

    #region AddAttribute

    #region Error

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddAttribute_WithNullClassId_ThrowsException()
    {
        _classServiceTest!.AddAttribute(null!, _testAttribute);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddAttribute_NullAttributeId_ThrowsException()
    {
        _classServiceTest!.AddAttribute(_testClass.Id, null!);
    }

    [TestMethod]
    public void AddAttribute_ClassIsInterface_ThrowsException()
    {
        var attributeId = Guid.NewGuid();
        var attribute = new Attribute
        {
            Id = attributeId,
            Name = "TestAttribute",
        };

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(_testInterfaceClass);

        Action action = () => _classServiceTest!.AddAttribute(_testInterfaceClass.Id, _testAttribute);
        action.Should().Throw<ArgumentException>().WithMessage("Cannot add attribute to an interface.");
    }

    [TestMethod]
    public void AddAttribute_AttributeRepeatedName_ThrowsException()
    {
        var attributeId = Guid.NewGuid();
        var attribute = new Attribute
        {
            Id = attributeId,
            Name = "TestAttribute",
        };

        var sameNameAttribute = new Attribute
        {
            Id = Guid.NewGuid(),
            Name = "TestAttribute",
        };

        _testClass.Attributes!.Add(sameNameAttribute);

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(_testClass);

        Action action = () => _classServiceTest!.AddAttribute(_testClass.Id, _testAttribute);
        action.Should().Throw<ArgumentException>().WithMessage("Attribute name already exists in class.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void AddAttribute_ValidAttribute_AddsAttribute()
    {
        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(_testClass);

        _classServiceTest!.AddAttribute(_testClass.Id, _testAttribute);
        _testClass.Attributes.Should().Contain(_testAttribute);
    }

    #endregion

    #endregion

    #region DeleteClass

    #region Error

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void DeleteClass_WithNullClassId_ThrowsException()
    {
        _classServiceTest!.DeleteClass(null!);
    }

    [TestMethod]
    public void DeleteClass_WhenClassDoesNotExist_ThrowsException()
    {
        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns((Class?)null);

        Action action = () => _classServiceTest!.DeleteClass(_testInterfaceClass.Id);
        action.Should().Throw<ArgumentException>().WithMessage("Class not found.");
    }

    [TestMethod]
    public void DeleteClass_WhenSomeOtherClassImplementsIt_ThrowsException()
    {
        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(_testInterfaceClass);

        _classRepositoryMock!
            .Setup(repo => repo.GetAll(It.IsAny<Func<Class, bool>>()))
            .Returns([_testClass]);

        _testClass.Parent = _testInterfaceClass;

        Action action = () => _classServiceTest!.DeleteClass(_testInterfaceClass.Id);
        action.Should().Throw<ArgumentException>().WithMessage("Cannot delete class that is implemented by another class.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void DeleteClass_WithValidClassIdAndIsNotImplemented_DeleteClass()
    {
        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(_testClass);

        _classRepositoryMock!
            .Setup(repo => repo.GetAll(It.IsAny<Func<Class, bool>>()))
            .Returns([]);

        _classRepositoryMock!
            .Setup(repo => repo.Delete(It.IsAny<Class>()));

        _classServiceTest!.DeleteClass(_testClass.Id);

        _classRepositoryMock!
            .Verify(repo => repo.Delete(It.IsAny<Class>()), Times.Once);
    }

    #endregion

    #endregion

    #region DeleteMethod

    #region Error

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void RemoveMethod_WithNullClassId_ThrowsException()
    {
        _classServiceTest!.RemoveMethod(null!, Guid.NewGuid());
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void RemoveMethod_WithNullMethodId_ThrowsException()
    {
        _classServiceTest!.RemoveMethod(Guid.NewGuid(), null!);
    }

    [TestMethod]
    public void RemoveMethod_WhenClassHasNoMethods_ThrowsException()
    {
        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(_testClass);

        Action action = () => _classServiceTest!.RemoveMethod(_testClass.Id, Guid.NewGuid());
        action.Should().Throw<ArgumentException>().WithMessage("Class has no methods.");
    }

    [TestMethod]
    public void RemoveMethod_WhenMethodIsNotInClass_ThrowsException()
    {
        var method = new Method
        {
            Name = "OtherTestMethod",
        };

        _testClass.Methods!.Add(method);

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(_testClass);

        Action action = () => _classServiceTest!.RemoveMethod(_testClass.Id, Guid.NewGuid());
        action.Should().Throw<ArgumentException>().WithMessage("Method not found in class.");
    }

    [TestMethod]
    public void RemoveMethod_WhenOtherMethodIsUsingIt_ThrowsException()
    {
        var method = new Method
        {
            Name = "TestMethod",
            MethodsInvoke = [_testMethod]
        };

        _testClass.Methods!.Add(method);
        _testClass.Methods!.Add(_testMethod);

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(_testClass);

        Action action = () => _classServiceTest!.RemoveMethod(_testClass.Id, _testMethod.Id);
        action.Should().Throw<ArgumentException>().WithMessage("Cannot remove method that is invoked by another method.");
    }

    [TestMethod]
    public void RemoveMethod_WhenIsImplementedInterfaceMethod_ThrowsException()
    {
        _testInterfaceClass.Methods!.Add(_testMethod);
        _testClass.Methods!.Add(_testMethod);
        _testClass.Parent = _testInterfaceClass;

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(_testClass);

        Action action = () => _classServiceTest!.RemoveMethod(_testClass.Id, _testMethod.Id);
        action.Should().Throw<ArgumentException>().WithMessage("Cannot remove method that is in an interface you implement.");
    }

    [TestMethod]
    public void RemoveMethod_WhenIsImplementedAbstractOverridingMethod_ThrowsException()
    {
        var abstractClass = new Class
        {
            Name = "AbstractClass",
            IsAbstract = true,
            IsInterface = false,
            IsSealed = false,
            Methods = [_testMethod],
            Attributes = [],
            Parent = null
        };

        _testClass.Parent = abstractClass;
        _testMethod.IsOverride = true;
        _testClass.Methods!.Add(_testMethod);

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(_testClass);

        Action action = () => _classServiceTest!.RemoveMethod(_testClass.Id, _testMethod.Id);
        action.Should().Throw<ArgumentException>().WithMessage("Cannot remove method that is overriding abstract parent method you implement.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void RemoveMethod_WhenCriteriaIsValid_DeleteMethod()
    {
        _testClass.Methods!.Add(_testMethod);

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(_testClass);

        _classServiceTest!.RemoveMethod(_testClass.Id, _testMethod.Id);

        _testClass.Methods.Should().NotContain(_testMethod);
    }

    [TestMethod]
    public void RemoveMethod_WhenCriteriaIsValidHasInterfaceParent_DeleteMethod()
    {
        _testClass.Methods!.Add(_testMethod);
        _testClass.Parent = _testInterfaceClass;

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(_testClass);

        _classServiceTest!.RemoveMethod(_testClass.Id, _testMethod.Id);

        _testClass.Methods.Should().NotContain(_testMethod);
    }

    [TestMethod]
    public void RemoveMethod_WhenCriteriaIsValidHasAbstractParent_DeleteMethod()
    {
        _testClass.Methods!.Add(_testMethod);

        var abstractClass = new Class
        {
            Name = "AbstractClass",
            IsAbstract = true,
            IsInterface = false,
            IsSealed = false,
            Methods = [],
            Attributes = [],
            Parent = null
        };

        _testClass.Parent = abstractClass;

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(_testClass);

        _classServiceTest!.RemoveMethod(_testClass.Id, _testMethod.Id);

        _testClass.Methods.Should().NotContain(_testMethod);
    }

    #endregion

    #endregion
}
