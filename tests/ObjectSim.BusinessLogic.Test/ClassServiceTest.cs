using FluentAssertions;
using Moq;
using ObjectSim.ClassConstructor.ClassBuilders;
using ObjectSim.ClassConstructor.ClassBuilders.Builders;
using ObjectSim.ClassConstructor.Strategy;
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
        var parentId = Guid.NewGuid();
        var argsWithNullAttributes = new CreateClassArgs(
            "TestClass",
            false,
            false,
            false,
            null!,
            [],
            null);

        var classBuilder = GetMockedBuilder();
        _builderStrategyMock!.Setup(x => x.WhichIsMyBuilder(It.IsAny<CreateClassArgs>())).Returns(true);
        _builderStrategyMock.Setup(x => x.CreateBuilder()).Returns(classBuilder);

        Action action = () => _classServiceTest!.CreateClass(argsWithNullAttributes);

        action.Should().Throw<ArgumentException>();
    }

    #endregion

    #region Success

    [TestMethod]
    public void SetParent_InvalidParentID_AddsNullParent()
    {
        var invalidParentId = Guid.NewGuid();
        var invalidParentClass = new Class()
        {
            Id = invalidParentId,
            Name = "InvalidClass",
            IsAbstract = false,
            IsSealed = false,
            IsInterface = false,
            Methods = [],
            Attributes = []
        };

        var classBuilder = GetMockedBuilder();
        _builderStrategyMock!.Setup(x => x.WhichIsMyBuilder(It.IsAny<CreateClassArgs>())).Returns(true);
        _builderStrategyMock.Setup(x => x.CreateBuilder()).Returns(classBuilder);

        _classRepositoryMock!.Setup(repo => repo.Get(It.Is<Func<Class, bool>>(f =>
                f.Invoke(invalidParentClass))))
            .Returns(invalidParentClass);

        _classRepositoryMock.Setup(repo => repo.Add(It.IsAny<Class>()))
            .Returns((Class c) => { c.Parent = null; return c; });

        _args.Parent = invalidParentClass.Id;

        var result = _classServiceTest!.CreateClass(_args);

        result.Parent.Should().BeNull();
    }

    [TestMethod]
    public void CreateClass_WithValidParameters_SetsStrategyBuilder()
    {
        var classBuilder = GetMockedBuilder();

        _args.Parent = null;

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

    #region RemoveAttribute

    #region Error

    [TestMethod]
    public void RemoveAttribute_WhenClassHasNoAttributes_ThrowsException()
    {
        var classWithoutAttributes = new Class
        {
            Name = "ClassNoAttributes",
            Attributes = []
        };

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(classWithoutAttributes);

        Action action = () => _classServiceTest!.RemoveAttribute(classWithoutAttributes.Id, Guid.NewGuid());
        action.Should().Throw<ArgumentException>().WithMessage("The class have no attributes.");
    }

    [TestMethod]
    public void RemoveAttribute_WhenAttributeNotInClass_ThrowsException()
    {
        var attribute = new Attribute
        {
            Id = Guid.NewGuid(),
            Name = "ExistingAttribute"
        };

        var classWithAttributes = new Class
        {
            Name = "ClassWithAttributes",
            Attributes = [attribute]
        };

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(classWithAttributes);

        Action action = () => _classServiceTest!.RemoveAttribute(classWithAttributes.Id, Guid.NewGuid());
        action.Should().Throw<ArgumentException>().WithMessage("That attribute does not exist in the class.");
    }

    [TestMethod]
    public void RemoveAttribute_WhenAttributeIsUsedInMethod_ThrowsException()
    {
        var attributeId = Guid.NewGuid();
        var attribute = new Attribute
        {
            Id = attributeId,
            Name = "UsedAttribute"
        };

        var localVariable = new ValueType()
        {
            Name = "UsedAttribute"
        };

        var method = new Method
        {
            Name = "Method",
            LocalVariables = [localVariable]
        };

        var classWithAttributeInUse = new Class
        {
            Name = "Class",
            Attributes = [attribute],
            Methods = [method]
        };

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(classWithAttributeInUse);

        Action action = () => _classServiceTest!.RemoveAttribute(classWithAttributeInUse.Id, attributeId);
        action.Should().Throw<ArgumentException>()
            .WithMessage("Attribute is being used in method.");
    }

    #endregion

    #region Success

    [TestMethod]
    public void RemoveAttribute_WhenCriteriaIsValid_DeletesAttribute()
    {
        var attributeId = Guid.NewGuid();
        var attribute = new Attribute
        {
            Id = attributeId,
            Name = "AttributeToRemove"
        };

        var classWithAttribute = new Class
        {
            Name = "ClassWithAttribute",
            Attributes = [attribute],
            Methods = []
        };

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(classWithAttribute);

        _classRepositoryMock!
            .Setup(repo => repo.Update(It.IsAny<Class>()))
            .Callback<Class>(c => {
                c.Attributes!.Should().NotContain(attribute);
            })
            .Returns((Class c) => c);

        _classServiceTest!.RemoveAttribute(classWithAttribute.Id, attributeId);

        _classRepositoryMock.Verify(repo => repo.Update(It.IsAny<Class>()), Times.Once);
    }

    [TestMethod]
    public void RemoveAttribute_WhenClassHasMethodsButAttributeNotUsed_DeletesAttribute()
    {
        var attributeId = Guid.NewGuid();
        var attribute = new Attribute
        {
            Id = attributeId,
            Name = "AttributeToRemove"
        };

        var method = new Method
        {
            Name = "TestMethod",
            LocalVariables = [new ValueType { Name = "DifferentName" }],
            Parameters = [new ValueType() { Name = "DifferentParam" }]
        };

        var classWithMethodsAndAttribute = new Class
        {
            Name = "Class",
            Attributes = [attribute],
            Methods = [method]
        };

        _classRepositoryMock!
            .Setup(repo => repo.Get(It.IsAny<Func<Class, bool>>()))
            .Returns(classWithMethodsAndAttribute);

        _classRepositoryMock!
            .Setup(repo => repo.Update(It.IsAny<Class>()))
            .Callback<Class>(c => {
                c.Attributes!.Should().NotContain(attribute);
            })
            .Returns((Class c) => c);

        _classServiceTest!.RemoveAttribute(classWithMethodsAndAttribute.Id, attributeId);

        _classRepositoryMock.Verify(repo => repo.Update(It.IsAny<Class>()), Times.Once);
    }

    #endregion

    #endregion
}
