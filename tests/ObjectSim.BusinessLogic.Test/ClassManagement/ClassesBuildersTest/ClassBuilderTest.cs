using ClassManagement.ClassesBuilders.Builders;
using FluentAssertions;

namespace ObjectSim.BusinessLogic.Test.ClassManagement.ClassesBuildersTest;

[TestClass]
public class ClassBuilderTest
{
    private ClassBuilder? _classBuilderTest;

    [TestInitialize]
    public void Initialize()
    {
        _classBuilderTest = new ClassBuilder();
    }

    #region Error

    [TestMethod]
    public void SetAttributes_Null_ThrowsException()
    {
        Action action = () => _classBuilderTest!.SetAttributes(null!);

        action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void SetMethods_Null_ThrowsException()
    {
        Action action = () => _classBuilderTest!.SetMethods(null!);

        action.Should().Throw<ArgumentNullException>();
    }

    #endregion
}
