using Moq;
using ObjectSim.BusinessLogic.ClassLogic.ClassBuilders.Builders;
using ObjectSim.IBusinessLogic;

namespace ObjectSim.BusinessLogic.Test.ClassLogic.ClassBuildersTest;

[TestClass]
public class InterfaceBuilderTest
{
    private InterfaceBuilder? _interfaceBuilderTest;
    private Mock<IMethodService>? _methodServiceMock;
    private Mock<IClassService>? _classServiceMock;
    private Mock<IAttributeService>? _attributeServiceMock;

    [TestInitialize]
    public void Initialize()
    {
        _methodServiceMock = new Mock<IMethodService>(MockBehavior.Strict);
        _classServiceMock = new Mock<IClassService>(MockBehavior.Strict);
        _attributeServiceMock = new Mock<IAttributeService>(MockBehavior.Strict);
        _interfaceBuilderTest = new InterfaceBuilder(_classServiceMock.Object);
    }

    #region Error

    [TestMethod]
    public void SetAttributes_SetsEmptyAttributes()
    {
        /*
        List<Attribute> attributes = new List<Attribute>();

        _interfaceBuilderTest!.SetAttributes(attributes);
        var result = _interfaceBuilderTest.GetResult();

        result.Attributes.Should().BeEmpty();
        */
    }

    #endregion
}
