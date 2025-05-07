using Moq;
using ObjectSim.DataAccess.Interface;
using ObjectSim.Domain;
using ObjectSim.Domain.Args;
using ValueType = ObjectSim.Domain.ValueType;

namespace ObjectSim.BusinessLogic.Test;

[TestClass]
public class DataTypeServiceTest
{
    private Mock<IRepository<Class>> _classRepositoryMock = null!;
    private DataTypeService _dataTypeServiceTest = null!;

    [TestInitialize]
    public void Setup()
    {
        _classRepositoryMock = new Mock<IRepository<Class>>();
        _dataTypeServiceTest = new DataTypeService(_classRepositoryMock.Object);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _classRepositoryMock.VerifyAll();
    }

    #region CreateDataType

    #region Error

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CreateDataType_ObjectReferenceTypeWithoutClass_ShouldThrow()
    {
        var args = new CreateDataTypeArgs("object", "Reference");
        _classRepositoryMock.Setup(r => r.GetAll(It.IsAny<Func<Class, bool>>()))
            .Returns([]);

        _dataTypeServiceTest.CreateDataType(args);
    }

    #endregion

    #region Success

    [TestMethod]
    public void CreateDataType_ReferenceType_ObjectWithRegisteredClass_ShouldSucceed()
    {
        var args = new CreateDataTypeArgs("object", "Reference");

        _classRepositoryMock
            .Setup(r => r.GetAll(It.IsAny<Func<Class, bool>>()))
            .Returns([new Class { Name = "object" }]);

        var result = _dataTypeServiceTest.CreateDataType(args);

        Assert.IsInstanceOfType(result, typeof(ReferenceType));
        Assert.AreEqual("object", result.Name);
        Assert.AreEqual("Reference", result.Type);
    }

    [TestMethod]
    public void CreateDataType_ObjectReferenceTypeWithClass_ShouldSucceed()
    {
        var args = new CreateDataTypeArgs("object", "Reference");

        _classRepositoryMock.Setup(r => r.GetAll(It.IsAny<Func<Class, bool>>()))
            .Returns([new Class { Name = "object" }]);

        var result = _dataTypeServiceTest.CreateDataType(args);

        Assert.IsInstanceOfType(result, typeof(ReferenceType));
        Assert.AreEqual("object", result.Name);
    }

    [TestMethod]
    public void CreateDataType_WithValidIntType_ShouldCreateValueType()
    {
        var args = new CreateDataTypeArgs("MyInt", "int");

        var result = _dataTypeServiceTest.CreateDataType(args);

        Assert.IsInstanceOfType(result, typeof(ValueType));
        Assert.AreEqual("MyInt", result.Name);
        Assert.AreEqual("int", result.Type);
        Assert.AreEqual(0, result.MethodIds.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CreateDataType_ShouldThrow_WhenReferenceTypeNotFoundInClasses()
    {
        var args = new CreateDataTypeArgs("InvalidRef", "UnknownClass");

        _classRepositoryMock.Setup(r => r.GetAll(It.IsAny<Func<Class, bool>>()))
            .Returns([]);

        _dataTypeServiceTest.CreateDataType(args);
    }

    [TestMethod]
    public void CreateDataType_ShouldReturnReferenceType_WhenTypeMatchesExistingClass()
    {
        var args = new CreateDataTypeArgs("MyClientRef", "Client");

        _classRepositoryMock.Setup(r => r.GetAll(It.IsAny<Func<Class, bool>>()))
            .Returns([new Class { Name = "Client" }]);

        var result = _dataTypeServiceTest.CreateDataType(args);

        Assert.IsInstanceOfType(result, typeof(ReferenceType));
        Assert.AreEqual("MyClientRef", result.Name);
        Assert.AreEqual("Client", result.Type);
    }

    [TestMethod]
    public void CreateDataType_WithStringType_ShouldCreateReferenceType()
    {
        var args = new CreateDataTypeArgs("MyString", "string");

        var result = _dataTypeServiceTest.CreateDataType(args);

        Assert.IsInstanceOfType(result, typeof(ReferenceType));
        Assert.AreEqual("MyString", result.Name);
        Assert.AreEqual("string", result.Type);
        Assert.AreEqual(0, result.MethodIds.Count);
    }

    #endregion

    #endregion

}
