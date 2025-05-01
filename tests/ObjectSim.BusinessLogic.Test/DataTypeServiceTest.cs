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
    }
}
