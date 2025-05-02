using Moq;
using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;

namespace ObjectSim.BusinessLogic.Test;
[TestClass]
public class DataTypeServiceTest
{
    private Mock<IRepository<Class>> _classRepoMock = null!;
    private DataTypeService _service = null!;

    [TestInitialize]
    public void Setup()
    {
        _classRepoMock = new Mock<IRepository<Class>>();
        _service = new DataTypeService(_classRepoMock.Object);
    }

    [TestMethod]
    public void CreateDataType_ReferenceType_ObjectWithRegisteredClass_ShouldSucceed()
    {
        var args = new CreateDataTypeArgs("object", "Reference");

        _classRepoMock
            .Setup(r => r.GetAll(It.IsAny<Func<Class, bool>>()))
            .Returns([new Class { Name = "object" }]);

        var result = _service.CreateDataType(args);

        Assert.IsInstanceOfType(result, typeof(ReferenceType));
        Assert.AreEqual("object", result.Name);
        Assert.AreEqual("Reference", result.Type);
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CreateDataType_ObjectReferenceTypeWithoutClass_ShouldThrow()
    {
        var args = new CreateDataTypeArgs("object", "Reference");
        _classRepoMock.Setup(r => r.GetAll(It.IsAny<Func<Class, bool>>()))
                      .Returns([]);

        _service.CreateDataType(args);
    }
    [TestMethod]
    public void CreateDataType_ObjectReferenceTypeWithClass_ShouldSucceed()
    {
        var args = new CreateDataTypeArgs("object", "Reference");

        _classRepoMock.Setup(r => r.GetAll(It.IsAny<Func<Class, bool>>()))
                      .Returns([new Class { Name = "object" }]);

        var result = _service.CreateDataType(args);

        Assert.IsInstanceOfType(result, typeof(ReferenceType));
        Assert.AreEqual("object", result.Name);
    }
    [TestMethod]
    public void CreateDataType_WithValidIntType_ShouldCreateValueType()
    {
        var args = new CreateDataTypeArgs("MyInt", "int");

        var result = _service.CreateDataType(args);

        Assert.IsInstanceOfType(result, typeof(Domain.ValueType));
        Assert.AreEqual("MyInt", result.Name);
        Assert.AreEqual("int", result.Type);
        Assert.AreEqual(0, result.MethodIds.Count);
    }

}
