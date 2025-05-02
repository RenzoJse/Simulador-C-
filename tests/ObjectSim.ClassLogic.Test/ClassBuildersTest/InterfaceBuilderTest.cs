using FluentAssertions;
using Moq;
using ObjectSim.ClassLogic.ClassBuilders.Builders;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ObjectSim.IBusinessLogic;
using Attribute = ObjectSim.Domain.Attribute;
using ValueType = System.ValueType;

namespace ObjectSim.ClassLogic.Test.ClassBuildersTest;

[TestClass]
public class InterfaceBuilderTest
{
    private InterfaceBuilder? _interfaceBuilderTest;
    private Mock<IMethodService>? _methodServiceMock;
    private Mock<IClassService>? _classServiceMock;
    private Mock<IAttributeService>? _attributeServiceMock;
    
    private static readonly CreateDataTypeArgs TestArgsDataType = new(
        "int", "reference");

    private static readonly Guid TestAttributeId = Guid.NewGuid();
    
    private readonly CreateAttributeArgs _testArgsAttribute = new(
        TestArgsDataType,
        "public",
        TestAttributeId,
        "Test"
    );

    [TestInitialize]
    public void Initialize()
    {
        _methodServiceMock = new Mock<IMethodService>(MockBehavior.Strict);
        _classServiceMock = new Mock<IClassService>(MockBehavior.Strict);
        _attributeServiceMock = new Mock<IAttributeService>(MockBehavior.Strict);
        _interfaceBuilderTest = new InterfaceBuilder(_classServiceMock.Object, _attributeServiceMock!.Object);
    }

    #region SetAttributes

    #region Error

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void SetAttributes_NullAttributeList_ThrowsException()
    {
        _interfaceBuilderTest!.SetAttributes(null!);
    }

    #endregion

    #region Success

    [TestMethod]
    public void SetAttributes_WithAttributeList_AddsEmptyAttributeList()
    {
        _interfaceBuilderTest!.SetAttributes([_testArgsAttribute]);
        var result = _interfaceBuilderTest.GetResult();

        result.Attributes.Should().BeEmpty();
    }

    #endregion
    
    #endregion
    
}
