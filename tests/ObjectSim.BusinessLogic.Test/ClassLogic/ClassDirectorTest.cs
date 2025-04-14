using ObjectSim.BusinessLogic.ClassLogic;
using ObjectSim.BusinessLogic.ClassLogic.ClassBuilders.Builders;

namespace ObjectSim.BusinessLogic.Test.ClassLogic;

[TestClass]
public class ClassDirectorTest
{
    private ClassDirector? _classDirector;

    [TestInitialize]
    public void Initialize()
    {
        var builder = new ClassBuilder();
        _classDirector = new ClassDirector(builder);
    }

}
