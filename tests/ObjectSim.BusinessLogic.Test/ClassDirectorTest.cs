using FluentAssertions;
using ObjectSim.BusinessLogic.ClassesBuilders.Builders;
using ObjectSim.Domain.Args;

namespace ObjectSim.BusinessLogic.Test;

public class ClassDirectorTest
{
    private ClassBuilder? _classBuilder;

    [TestInitialize]
    public void Initialize()
    {
        _classBuilder = new ClassBuilder();
    }

    #region Create

    #region Error

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CreateClass_WithoutName_ThrowsException()
    {
        var args = new CreateClassArgs(null, true, true, [], [], null!, Guid.NewGuid());

        ClassService.Create(args, _classBuilder!);
    }

    [TestMethod]
    public void CreateClass_WithNameLongerThan15Characters_ThrowsException()
    {
        const string longName = "15CharactersLongName";
        var args = new CreateClassArgs(longName, true, true, [], [], null!, Guid.NewGuid());

        Action action = () => ClassService.Create(args, _classBuilder!);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot be longer than 15 characters");
    }

    [TestMethod]
    public void CreateClass_WithNameShorterThan3Characters_ThrowsException()
    {
        const string shortName = "ab";
        var args = new CreateClassArgs(shortName, true, true, [], [], null!, Guid.NewGuid());

        Action action = () => ClassService.Create(args, _classBuilder!);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot be shorter than 3 characters");
    }

    #endregion

    #endregion
}
