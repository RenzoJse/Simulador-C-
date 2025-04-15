using FluentAssertions;
using Moq;
using ObjectSim.BusinessLogic.ClassLogic.ClassBuilders;
using ObjectSim.BusinessLogic.ClassLogic.ClassBuilders.Builders;
using ObjectSim.Domain;
using ObjectSim.IDataAccess;
using Attribute = ObjectSim.Domain.Attribute;

namespace ObjectSim.BusinessLogic.Test.ClassLogic.ClassBuildersTest;

[TestClass]
public class BuilderTest
{
    private Builder? _builder;
    private Mock<IMethodRepository<Method>>? _methodRepositoryMock;
    private MethodService? _methodService;

    [TestInitialize]
    public void Initialize()
    {
        _builder = new ClassBuilder();
        _methodRepositoryMock = new Mock<IMethodRepository<Method>>(MockBehavior.Strict);
        _methodService = new MethodService(_methodRepositoryMock.Object);
    }

    #region Error

    [TestMethod]
    public void CreateClass_WithNullName_ThrowsException()
    {
        Action action = () => _builder!.SetName(null!);

        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void CreateClass_WithNullAbstraction_ThrowsException()
    {
        Action action = () => _builder!.SetAbstraction(null!);

        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void CreateClass_WithNullSealed_ThrowsException()
    {
        Action action = () => _builder!.SetSealed(null!);

        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void CreateClass_WithNullAttributes_ThrowsException()
    {
        Action action = () => _builder!.SetAttributes(null!);

        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void CreateClass_WithNotExistingAttributes_ThrowsException()
    {
        /*
        var attributeId = Guid.NewGuid();
        _methodRepositoryMock!.Setup(x => x.GetById(attributeId)).Returns(((Attribute?)null)!);

        Action action = () => _builder!.SetAttributes([attributeId]);

        action.Should().Throw<ArgumentException>();
        */
    }

    [TestMethod]
    public void CreateClass_WithAttributesEmptyList_ThrowsException()
    {
        Action action = () => _builder!.SetAttributes([]);
        action.Should().Throw<ArgumentException>("The list of attributes cannot be empty");
    }

    [TestMethod]
    public void CreateClass_WithNullMethods_ThrowsException()
    {
        Action action = () => _builder!.SetMethods(null!);

        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void CreateClass_WithNotExistingMethods_ThrowsException()
    {
        var methodId = Guid.NewGuid();
        _methodRepositoryMock!.Setup(x => x.GetById(methodId)).Returns(((Method?)null)!);

        Action action = () => _builder!.SetMethods([methodId]);

        action.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void CreateClass_WithMethodsEmptyList_ThrowsException()
    {
        Action action = () => _builder!.SetMethods([]);
        action.Should().Throw<ArgumentException>("The list of methods cannot be empty");
    }

    [TestMethod]
    public void CreateClass_GetResultWhenCreationIsNull_ThrowsException()
    {
        Action action = () => _builder!.GetResult();

        action.Should().Throw<ArgumentNullException>();
    }

    #endregion

    #region Success

    [TestMethod]
    public void CreateClass_WithValidName_SetsName()
    {
        _builder!.SetName("ValidName");
        _builder.GetResult().Name.Should().Be("ValidName");
    }

    [TestMethod]
    public void CreateClass_WithNullAbstraction_SetsAbstraction()
    {
        _builder!.SetAbstraction(true);
        _builder.GetResult().IsAbstract.Should().BeTrue();
    }

    [TestMethod]
    public void CreateClass_WithValidSealed_SetsSealed()
    {
        _builder!.SetSealed(true);
        _builder.GetResult().IsSealed.Should().BeTrue();
    }

    [TestMethod]
    public void CreateClass_WithValidAttributes_SetsAttributes()
    {
    }

    [TestMethod]
    public void CreateClass_WithValidMethods_SetsMethods()
    {
    }

    [TestMethod]
    public void CreateClass_WithValidParent_SetsParent()
    {
    }

    [TestMethod]
    public void CreateClass_GetResultWhenCreationIsValid_ReturnsClass()
    {
        _builder!.SetName("ValidName");
        _builder.SetAbstraction(true);
        _builder.SetSealed(true);
        _builder.SetAttributes([Guid.NewGuid()]);
        _builder.SetMethods([Guid.NewGuid()]);
        _builder.SetParent(Guid.NewGuid());

        var result = _builder.GetResult();

        result.Should().NotBeNull();
        result.Name.Should().Be("ValidName");
        result.IsAbstract.Should().BeTrue();
        result.IsSealed.Should().BeTrue();
    }

    #endregion

}
