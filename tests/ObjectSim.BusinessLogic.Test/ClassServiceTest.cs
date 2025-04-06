using ObjectSim.BusinessLogic.Args;
using FluentAssertions;

namespace ObjectSim.BusinessLogic.Test;

[TestClass]
public class ClassServiceTest
{
    private ClassService? _classService;

    [TestInitialize]
    public void Initialize()
    {
        _classService = new ClassService();
    }

    #region Create
    #region Error

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CreateClass_WithoutName_ThrowsException()
    {
        var args = new CreateClassArgs(null, true, true, [],[], null!, Guid.NewGuid());

        ClassService.Create(args);
    }

    [TestMethod]
    public void CreateClass_WithNameLongerThan15Characters_ThrowsException()
    {
        var longName = "15CharactersLongName";
        var args = new CreateClassArgs(longName, true, true, [],[], null!, Guid.NewGuid());

        Action action = () => ClassService.Create(args);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot be longer than 15 characters");
    }

    [TestMethod]
    public void CreateClass_WithNameShorterThan3Characters_ThrowsException()
    {
        var shortName = "ab";
        var args = new CreateClassArgs(shortName, true, true, [],[], null!, Guid.NewGuid());

        Action action = () => ClassService.Create(args);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot be shorter than 3 characters");
    }

    #endregion
    #endregion
}
