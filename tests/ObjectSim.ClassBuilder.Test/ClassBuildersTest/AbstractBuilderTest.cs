using ObjectSim.ClassConstructor.ClassBuilders.Builders;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;

namespace ObjectSim.ClassLogic.Test.ClassBuildersTest;

[TestClass]
public class AbstractBuilderTest
{

    #region SetAttributes

    #region MyRegion

    [TestMethod]
    public void SetAttributes_ShouldNotThrow_WhenCalledWithEmptyList()
    {
        var builder = new AbstractBuilder();
        var attributes = new List<CreateAttributeArgs>();
        builder.SetAttributes(attributes);
    }

    #endregion

    #endregion

    #region SetMethods

    [TestMethod]
    public void SetMethods_ShouldNotThrow_WhenCalledWithEmptyList()
    {
        var builder = new AbstractBuilder();
        var methods = new List<CreateMethodArgs>();
        builder.SetMethods(methods);
    }

    #endregion

}
