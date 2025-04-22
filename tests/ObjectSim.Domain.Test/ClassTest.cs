using FluentAssertions;

namespace ObjectSim.Domain.Test;

[TestClass]
public class ClassTest
{
    private Class? _testClass;

    [TestInitialize]
    public void Initialize()
    {
        _testClass = new Class();
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
}
