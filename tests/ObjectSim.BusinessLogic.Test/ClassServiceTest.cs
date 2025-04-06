using ObjectSim.BusinessLogic.Args;

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
    public void CreateClass_WithoutName_ThrowsException()
    {
        var args = new CreateClassArgs(null, true, true, [],[], null!, Guid.NewGuid());

        _classService!.Create(args);
    }

    #endregion
    #endregion
}
