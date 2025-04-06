namespace ObjectSim.BusinessLogic.Test;

[TestClass]
public class ClassServiceTest
{
    #region Create
    #region Error

    [TestMethod]
    public void CreateClass_WithoutName_ThrowsException()
    {
        var args = new CreateClassArgs(Name: null, IsAbstract: "Description", IsSealed: true, Attribute: new List<Attributes>(), Methods: new List<Method>(), Parent: Class, Id: Guid.NewGuid());

        _classService.CreateClass(args);
    }

    #endregion
    #endregion
}
