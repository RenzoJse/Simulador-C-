using FluentAssertions;
using ObjectSim.BusinessLogic.ClassesBuilders;
using ObjectSim.BusinessLogic.ClassesBuilders.Builders;
using ObjectSim.Domain.Args;

namespace ObjectSim.BusinessLogic.Test.ClassesBuildersTest;

[TestClass]
public class BuilderTest
{
    private Builder? _genericBuilder;

    [TestInitialize]
    public void Initialize()
    {
        _genericBuilder = new ClassBuilder();
    }

    #region CreateClass

    #region Error

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CreateClass_WithoutName_ThrowsException()
    {
        _genericBuilder!.SetName(null!);
    }

    [TestMethod]
    public void CreateClass_WithNameLongerThan15Characters_ThrowsException()
    {
        const string longName = "15CharactersLongName";

        Action action = () => _genericBuilder!.SetName(longName);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot be longer than 15 characters");
    }

    [TestMethod]
    public void CreateClass_WithNameShorterThan3Characters_ThrowsException()
    {
        const string shortName = "ab";

        Action action = () => _genericBuilder!.SetName(shortName);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot be shorter than 3 characters");
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CreateClass_SetAbstractionNull_ThrowsException()
    {
        _genericBuilder!.SetAbstraction(null!);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CreateClass_SetSealedNull_ThrowsException()
    {
        _genericBuilder!.SetSealed(null!);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CreateClass_WithNullAttributes_ThrowsException()
    {
        _genericBuilder!.SetAttributes(null!);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CreateClass_WithNullMethods_ThrowsException()
    {
        _genericBuilder!.SetMethods(null!);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CreateClass_WithNullParent_ThrowsException()
    {
        _genericBuilder!.SetParent(null!);
    }

    #endregion

    #endregion
}
